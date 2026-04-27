import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';
import { CreateProductRequest, EditProductRequest, ProductModel } from '../models/product';
import { OrganizationService } from './organization-service';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);

  readonly selectedProduct = signal<ProductModel | undefined>(undefined);

  readonly products = signal<ProductModel[]>([]);

  replaceInList(product: ProductModel) {
    this.products.update((products) => {
      const index = products.findIndex((p) => p.id === product.id);
      if (index === -1) return products;

      const updated = [...products];
      updated[index] = product;
      return updated;
    });
  }

  create(payload: CreateProductRequest): Observable<void> {
    return this.http.post<void>(`${this.api}/product`, payload);
  }

  edit(payload: EditProductRequest, id: string): Observable<void> {
    return this.http.put<ProductModel>(`${this.api}/product/${id}`, payload).pipe(
      tap((product) => {
        console.log(product);
        this.replaceInList(product);
      }),
      map(() => void 0),
    );
  }

  getList() {
    return this.http.get<ProductModel[]>(`${this.api}/product/list`).pipe(
      tap((list) => {
        this.products.set(list);
        console.log(this.products());
      }),
    );
  }
}
