import { Injectable, signal, computed, inject } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LoginRequest, AuthResponse, RegisterRequest } from '../models/auth';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private api = environment.apiUrl;
  private readonly http = inject(HttpClient);

  private readonly accessTokenSignal = signal<string | null>(null);

  isAuthenticated() {
    return !!this.accessTokenSignal() || sessionStorage.getItem('isLoggedIn') === 'true';
  }

  getAccessToken() {
    return this.accessTokenSignal();
  }

  login(payload: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.api}/auth/login`, payload).pipe(
      tap((response) => {
        this.accessTokenSignal.set(response.accessToken);
        sessionStorage.setItem('isLoggedIn', 'true');
      }),
    );
  }

  register(payload: RegisterRequest) {
    return this.http.post<AuthResponse>(`${this.api}/auth/register`, payload).pipe(
      tap((response) => {
        this.accessTokenSignal.set(response.accessToken);
        sessionStorage.setItem('isLoggedIn', 'true');
      }),
    );
  }

  refresh() {
    return this.http.post<AuthResponse>(`${this.api}/auth/refresh`, {}).pipe(
      tap((response) => {
        this.accessTokenSignal.set(response.accessToken);
        sessionStorage.setItem('isLoggedIn', 'true');
      }),
    );
  }

  logout() {
    this.accessTokenSignal.set(null);
    sessionStorage.removeItem('isLoggedIn');
    return this.http.post<AuthResponse>(`${this.api}/auth/logout`, {});
  }
}
