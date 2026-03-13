import { HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Auth } from '../services/auth';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(Auth);
  const reqWithToken = addToken(req, auth.getAccessToken());

  return next(reqWithToken).pipe(
    catchError((error) => {
      // Auto-refresh on 401 (expired access token) if url is not "refresh" to avoid looping
      if (error.status === 401 && !req.url.includes('/auth/refresh')) {
        return auth.refresh().pipe(
          switchMap((res) => {
            // Retry original request with new access token
            return next(addToken(req, res.accessToken));
          }),
          catchError((refreshError) => {
            // Log out if refresh token has expired in the meantime
            auth.logout();
            return throwError(() => refreshError);
          }),
        );
      }
      return throwError(() => error);
    }),
  );
};

function addToken(req: HttpRequest<unknown>, token: string | null) {
  if (!token) return req;
  return req.clone({
    setHeaders: { Authorization: `Bearer ${token}` },
    withCredentials: true, // Refresh token in HTTP Cookie
  });
}
