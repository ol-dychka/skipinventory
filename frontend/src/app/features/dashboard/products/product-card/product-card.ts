import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ProductModel } from '../../../../core/models/product';
import { ProductService } from '../../../../core/services/product-service';

@Component({
  selector: 'app-product-card',
  imports: [],
  templateUrl: './product-card.html',
})
export class ProductCard {
  @Input() product!: ProductModel;

  @Output() productSelect = new EventEmitter<ProductModel>();

  formatDate(date: string) {
    return new Date(date).toLocaleString();
  }
}
