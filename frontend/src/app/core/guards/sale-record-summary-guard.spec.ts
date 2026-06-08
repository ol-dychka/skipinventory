import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { saleRecordSummaryGuard } from './sale-record-summary-guard';

describe('saleRecordSummaryGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => saleRecordSummaryGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
