import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { orgGuardGuard } from './org-guard-guard';

describe('orgGuardGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => orgGuardGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
