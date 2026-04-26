import { Component, effect, inject, Input, signal, OnInit, input } from '@angular/core';
import { ScaledInput } from '../../../../shared/components/scaled-input/scaled-input';
import { ProductModel } from '../../../../core/models/product';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../../../core/services/product-service';
import { filter, finalize } from 'rxjs';
import { toObservable, takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { isActive } from '@angular/router';

@Component({
  selector: 'app-product-edit-panel',
  imports: [ScaledInput, ReactiveFormsModule],
  templateUrl: './product-edit-panel.html',
})
export class ProductEditPanel implements OnInit {
  private fb = inject(FormBuilder);
  private productService = inject(ProductService);

  product = input.required<ProductModel>();

  readonly loading = signal(false);

  form = this.fb.nonNullable.group({
    name: ['', [Validators.required]],
    sku: [''],
    vendor: ['', [Validators.required]],
    costPrice: [0, [Validators.required, Validators.min(0.01), Validators.pattern(/^\d+\.\d\d$/)]],
    salePrice: [0, [Validators.required, Validators.min(0.01), Validators.pattern(/^\d+\.\d\d$/)]],
    currentStock: [0, [Validators.required, Validators.min(1), Validators.pattern(/^\d+$/)]],
    reorderPoint: [0, [Validators.pattern(/^\d+$/)]],
    baseReorderQuantity: [1, [Validators.required, Validators.min(1), Validators.pattern(/^\d+$/)]],
    deliveryDelay: [0, [Validators.pattern(/^\d+$/)]],
    category: [''],
    isActive: [true],
  });

  ngOnInit() {
    this.form.patchValue({
      name: this.product().name,
      sku: this.product().sku,
      vendor: this.product().vendor,
      costPrice: this.product().costPrice,
      salePrice: this.product().salePrice,
      currentStock: this.product().currentStock,
      reorderPoint: this.product().reorderPoint,
      baseReorderQuantity: this.product().baseReorderQuantity,
      deliveryDelay: this.product().deliveryDelay,
      category: this.product().category,
      isActive: this.product().isActive,
    });
  }

  fieldError(name: string): string | null {
    const ctrl = this.form.get(name);
    if (!ctrl?.touched || !ctrl.errors) return null;
    const errors = ctrl.errors!;

    if (errors['required']) return 'This field is required';
    if (errors['min']) return `Minimum value is ${errors['min'].min}`;
    // if (errors['max']) return `Maximum value is ${errors['max'].max}`;
    // if (errors['minlength']) return `Minimum ${errors['minlength'].requiredLength} characters`;
    // if (errors['maxlength']) return `Maximum ${errors['maxlength'].requiredLength} characters`;
    // if (errors['email']) return 'Enter a valid email';
    if (errors['pattern']) return 'Invalid format';
    if (errors['notInteger']) return 'Must be a whole number';
    return null;
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    this.productService
      .edit(this.form.getRawValue())
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe();
  }
}
