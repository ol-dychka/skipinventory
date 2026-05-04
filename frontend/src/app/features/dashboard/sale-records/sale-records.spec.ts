import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleRecords } from './sale-records';

describe('SaleRecords', () => {
  let component: SaleRecords;
  let fixture: ComponentFixture<SaleRecords>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SaleRecords],
    }).compileComponents();

    fixture = TestBed.createComponent(SaleRecords);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
