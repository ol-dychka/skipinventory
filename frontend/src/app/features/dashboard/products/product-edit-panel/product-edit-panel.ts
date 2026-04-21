import { Component, Input } from '@angular/core';
import { ScaledInput } from '../../../../shared/components/scaled-input/scaled-input';
import { ProductModel } from '../../../../core/models/product';

@Component({
  selector: 'app-product-edit-panel',
  imports: [ScaledInput],
  templateUrl: './product-edit-panel.html',
})
export class ProductEditPanel {
  @Input() product!: ProductModel;
}
