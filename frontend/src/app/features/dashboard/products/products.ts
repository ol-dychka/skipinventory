import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProductService } from '../../../core/services/product-service';
import { ScaledInput } from '../../../shared/components/scaled-input/scaled-input';
import { ProductCard } from './product-card/product-card';
import { ProductModel } from '../../../core/models/product';
import { ProductEditPanel } from './product-edit-panel/product-edit-panel';

@Component({
  selector: 'app-products',
  imports: [RouterLink, ProductCard, ProductEditPanel],
  templateUrl: './products.html',
})
export class Products {
  productService = inject(ProductService);

  onProductSelect(p: ProductModel) {
    this.productService.selectedProduct.set(p);
    console.log(this.productService.selectedProduct());
  }
}
