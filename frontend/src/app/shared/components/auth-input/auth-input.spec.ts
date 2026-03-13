import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthInput } from './auth-input';

describe('AuthInput', () => {
  let component: AuthInput;
  let fixture: ComponentFixture<AuthInput>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthInput],
    }).compileComponents();

    fixture = TestBed.createComponent(AuthInput);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
