import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateProductRequest } from '../models/product';
import { OrganizationService } from './organization-service';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);

  create(payload: CreateProductRequest): Observable<void> {
    return this.http.post<void>(`${this.api}/organization`, payload);
  }
}
