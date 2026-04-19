import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScaledInput } from './scaled-input';

describe('ScaledInput', () => {
  let component: ScaledInput;
  let fixture: ComponentFixture<ScaledInput>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScaledInput],
    }).compileComponents();

    fixture = TestBed.createComponent(ScaledInput);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
