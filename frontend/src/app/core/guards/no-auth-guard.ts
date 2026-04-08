import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { catchError, map, of } from 'rxjs';

export const noAuthGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.currentUser()) {
    router.navigate(['/choose-organization']);
    return false;
  }

  return authService.refresh().pipe(
    map(() => {
      router.navigate(['/choose-organization']);
      return false;
    }),
    catchError(() => of(true)),
  );
};
