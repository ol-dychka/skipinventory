import { Injectable, signal, computed, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthApi, LoginRequest } from '../services/auth-api';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private readonly api = inject(AuthApi);
  private readonly router = inject(Router);

  private readonly accessTokenSignal = signal<string | null>(null);

  readonly isAuthenticated = computed(() => !!this.accessTokenSignal());

  login(payload: LoginRequest) {
    return this.api.login(payload).pipe(
      tap((response) => {
        this.accessTokenSignal.set(response.accessToken);

        localStorage.setItem('access_token', response.accessToken);
        localStorage.setItem('refresh_token', response.refreshToken);

        this.router.navigate(['/dashboard']);
      }),
    );
  }

  logout(): void {
    this.accessTokenSignal.set(null);

    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');

    // get backend to logout as well

    this.router.navigate(['/login']);
  }

  getAccessToken(): string | null {
    return this.accessTokenSignal();
  }

  restoreSession(): void {
    const token = localStorage.getItem('access_token');

    if (token) {
      this.accessTokenSignal.set(token);
    }
  }
}
