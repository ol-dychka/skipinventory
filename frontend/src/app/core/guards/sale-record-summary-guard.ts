import { CanActivateFn } from '@angular/router';
import { SaleRecordService } from '../services/sale-record-service';
import { inject } from '@angular/core';
import { catchError, map, of } from 'rxjs';

export const saleRecordSummaryGuard: CanActivateFn = (route, state) => {
  const saleRecordService = inject(SaleRecordService);

  if (saleRecordService.saleRecordSummary()) {
    return true;
  }

  return saleRecordService.getSummaryFromDateRange().pipe(
    map(() => true),
    catchError(() => {
      console.log('error getting products');
      return of(false);
    }),
  );
};
