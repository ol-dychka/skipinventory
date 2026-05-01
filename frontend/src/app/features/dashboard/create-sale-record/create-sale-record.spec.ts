import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSaleRecord } from './create-sale-record';

describe('CreateSaleRecord', () => {
  let component: CreateSaleRecord;
  let fixture: ComponentFixture<CreateSaleRecord>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateSaleRecord],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateSaleRecord);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
