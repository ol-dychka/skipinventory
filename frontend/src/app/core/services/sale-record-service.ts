import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { SaleBatchCreateRequest } from '../models/sale-record';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SaleRecordService {
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);

  create(payload: SaleBatchCreateRequest): Observable<void> {
    return this.http.post<void>(`${this.api}/salerecord`, payload);
  }

  exists(date: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.api}/salerecord/${date}`);
  }
}
