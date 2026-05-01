import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../../core/services/product-service';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgForOf } from '@angular/common';

@Component({
  selector: 'app-create-sale-record',
  imports: [ReactiveFormsModule],
  templateUrl: './create-sale-record.html',
})
export class CreateSaleRecord implements OnInit {
  private productService = inject(ProductService);
  private fb = inject(FormBuilder);

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
    console.log(this.form.value.items);
  }
}
