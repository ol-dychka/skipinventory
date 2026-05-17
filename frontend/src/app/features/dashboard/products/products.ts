import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProductService } from '../../../core/services/product-service';
import { ScaledInput } from '../../../shared/components/scaled-input/scaled-input';
import { ProductCard } from './product-card/product-card';
import { ProductModel } from '../../../core/models/product';
import { ProductEditPanel } from './product-edit-panel/product-edit-panel';
import { ProductCreatePanel } from './product-create-panel/product-create-panel';

@Component({
  selector: 'app-products',
  imports: [ProductCard, ProductEditPanel, ProductCreatePanel],
  templateUrl: './products.html',
})
export class Products {
  productService = inject(ProductService);

  isCreateOpen = signal(false);

  onProductSelect(p: ProductModel) {
    this.isCreateOpen.set(false);
    this.productService.selectedProduct.set(p);
    console.log(this.productService.selectedProduct());
  }
}
