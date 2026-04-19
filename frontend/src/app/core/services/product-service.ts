import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { CreateProductRequest, ProductModel } from '../models/product';
import { OrganizationService } from './organization-service';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);

  readonly products = signal<ProductModel[]>([]);

  create(payload: CreateProductRequest): Observable<void> {
    return this.http.post<void>(`${this.api}/product`, payload);
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
