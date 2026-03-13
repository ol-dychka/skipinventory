import { CanActivateFn, Router } from '@angular/router';
import { Auth } from '../services/auth';
import { inject } from '@angular/core';
import { catchError, map, of } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const auth = inject(Auth);
  const router = inject(Router);

  if (auth.isAuthenticated()) return true;

  return auth.refresh().pipe(
    map(() => true),
    catchError(() => {
      router.navigate(['/login'], {
        queryParams: { returnUrl: state.url },
      });
      return of(false);
    }),
  );
};
