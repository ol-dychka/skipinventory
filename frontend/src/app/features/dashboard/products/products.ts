import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProductService } from '../../../core/services/product-service';
import { ScaledInput } from '../../../shared/components/scaled-input/scaled-input';

@Component({
  selector: 'app-products',
  imports: [RouterLink, ScaledInput],
  templateUrl: './products.html',
})
export class Products {
  productService = inject(ProductService);

  isDisabled = signal(true);
}
