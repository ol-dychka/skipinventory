import { Injectable, signal, computed, inject } from '@angular/core';
import {
  BehaviorSubject,
  catchError,
  filter,
  finalize,
  map,
  Observable,
  switchMap,
  take,
  tap,
  throwError,
} from 'rxjs';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LoginRequest, AuthResponse, RegisterRequest, RefreshRequest } from '../models/auth';
import { UserModel, UserResponse } from '../models/user';
import { OrganizationService } from './organization-service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);
  // private readonly organizationService = inject(OrganizationService);

  readonly currentUser = signal<UserModel | null>(null);

  private isRefreshing = false;
  private refreshSubject = new BehaviorSubject<string | null>(null);

  getAccessToken(): string | null {
    return sessionStorage.getItem('access_token');
  }

  private setAccessToken(token: string): void {
    sessionStorage.setItem('access_token', token);
  }

  clearSession(): void {
    sessionStorage.removeItem('access_token');
    this.currentUser.set(null);
  }

  private applySession(token: string): Observable<string> {
    this.setAccessToken(token);
    console.log('token stored:', sessionStorage.getItem('access_token'));
    return this.http.get<UserResponse>(`${this.api}/user/details`, { withCredentials: true }).pipe(
      tap(({ user }) => {
        this.currentUser.set(user);
        console.log(user);
      }),
      map(() => token),
    );
  }

  private handleSessionError(err: unknown): Observable<never> {
    this.clearSession();
    return throwError(() => err);
  }

  login(payload: LoginRequest): Observable<string> {
    return this.http
      .post<AuthResponse>(`${this.api}/auth/login`, payload, { withCredentials: true })
      .pipe(
        switchMap(({ accessToken }) => this.applySession(accessToken)),
        catchError((err) => this.handleSessionError(err)),
      );
  }

  register(payload: RegisterRequest): Observable<string> {
    return this.http
      .post<AuthResponse>(`${this.api}/auth/register`, payload, { withCredentials: true })
      .pipe(
        switchMap(({ accessToken }) => this.applySession(accessToken)),
        catchError((err) => this.handleSessionError(err)),
      );
  }

  refresh(payload: RefreshRequest): Observable<string> {
    return this.http
      .post<AuthResponse>(`${this.api}/auth/refresh`, payload, { withCredentials: true })
      .pipe(
        switchMap(({ accessToken }) => this.applySession(accessToken)),
        catchError((err) => this.handleSessionError(err)),
      );
  }

  logout() {
    return this.http
      .post<AuthResponse>(`${this.api}/auth/logout`, {})
      .pipe(finalize(() => this.clearSession()));
  }

  handleUnauthorized(organizationId?: string): Observable<string> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshSubject.next(null);

      // case: token expires mid-way into organization workflow
      // this is the only method that refreshes token with organization data
      const payload: RefreshRequest = {
        organizationId: organizationId,
      };

      return this.http
        .post<AuthResponse>(`${this.api}/auth/refresh`, payload, { withCredentials: true })
        .pipe(
          switchMap(({ accessToken }) => {
            this.isRefreshing = false;
            this.refreshSubject.next(accessToken);
            return this.applySession(accessToken);
          }),
          catchError((err) => {
            this.isRefreshing = false;
            this.refreshSubject.next(null);
            return this.handleSessionError(err);
          }),
          finalize(() => {
            this.isRefreshing = false;
          }),
        );
    }

    return this.refreshSubject.pipe(
      filter((token) => token !== null),
      take(1),
      map((token) => token as string),
    );
  }
}
