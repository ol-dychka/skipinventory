import { Component, inject, OnInit, signal } from '@angular/core';
import { ProductService } from '../../../core/services/product-service';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SaleRecordService } from '../../../core/services/sale-record-service';
import { SaleBatchCreateRequest } from '../../../core/models/sale-record';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-create-sale-record',
  imports: [ReactiveFormsModule],
  templateUrl: './create-sale-record.html',
})
export class CreateSaleRecord implements OnInit {
  private productService = inject(ProductService);
  private saleRecordService = inject(SaleRecordService);
  private fb = inject(FormBuilder);

  loading = signal(false);

  form!: FormGroup;

  ngOnInit(): void {
    this.form = this.fb.group({
      items: this.fb.array(
        this.productService.products().map((product) =>
          this.fb.group({
            id: [product.id],
            name: [product.name],
            sku: [product.sku],
            quantity: [0, [Validators.required]],
          }),
        ),
      ),
    });
  }

  get itemControls() {
    return (this.form.get('items') as FormArray).controls;
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      console.log('invalid form');
      return;
    }

    console.log('a');

    this.loading.set(true);

    const payload: SaleBatchCreateRequest = {
      data: this.form.value.items,
      date: undefined,
    };

    this.saleRecordService
      .create(payload)
      .pipe(
        finalize(() => {
          this.loading.set(false);
          console.log('sale record submission done');
        }),
      )
      .subscribe();
  }
}
