import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { forecastGuard } from './forecast-guard';

describe('forecastGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => forecastGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
