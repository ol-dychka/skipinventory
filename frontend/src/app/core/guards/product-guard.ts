import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ProductService } from '../services/product-service';
import { catchError, map, of } from 'rxjs';

export const productGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const productService = inject(ProductService);

  if (productService.products().length > 0) {
    return true;
  }

  return productService.getList().pipe(
    map(() => true),
    catchError(() => {
      console.log('error getting products');
      router.navigate(['/choose-organization'], { queryParams: { returnUrl: state.url } });
      return of(false);
    }),
  );
};
