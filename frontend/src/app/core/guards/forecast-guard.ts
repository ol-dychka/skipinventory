import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ModelTrainingService } from '../services/model-training-service';
import { catchError, map, of } from 'rxjs';

export const forecastGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const mlService = inject(ModelTrainingService);

  if (mlService.forecasts().length > 0) {
    return true;
  }

  return mlService.list().pipe(
    map(() => true),
    catchError(() => {
      console.log('error getting forecasts');
      router.navigate(['/dashboard'], { queryParams: { returnUrl: state.url } });
      return of(false);
    }),
  );
};
