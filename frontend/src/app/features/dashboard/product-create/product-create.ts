import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../../core/services/product-service';
import { AuthInput } from '../../../shared/components/auth-input/auth-input';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-product-create',
  imports: [ReactiveFormsModule, AuthInput],
  templateUrl: './product-create.html',
})
export class ProductCreate {
  private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private router = inject(Router);

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
  });

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
      .create(this.form.getRawValue())
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(() => this.router.navigate(['../dashboard']));
  }
}
