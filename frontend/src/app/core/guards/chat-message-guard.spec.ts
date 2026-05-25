import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { chatMessageGuard } from './chat-message-guard';

describe('chatMessageGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => chatMessageGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
