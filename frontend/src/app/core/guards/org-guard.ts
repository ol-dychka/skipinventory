import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { OrganizationService } from '../services/organization-service';
import { catchError, map, of } from 'rxjs';

export const orgGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const organizationService = inject(OrganizationService);

  const organizationId = route.paramMap.get('organizationId');

  if (organizationService.currentOrganization()) {
    return true;
  }

  if (!organizationId) return false;

  return organizationService.access(organizationId).pipe(
    map(() => true),
    catchError(() => {
      console.log('NIGA');
      router.navigate(['/choose-organization'], { queryParams: { returnUrl: state.url } });
      return of(false);
    }),
  );
};
