import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductEditPanel } from './product-edit-panel';

describe('ProductEditPanel', () => {
  let component: ProductEditPanel;
  let fixture: ComponentFixture<ProductEditPanel>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductEditPanel],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductEditPanel);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
