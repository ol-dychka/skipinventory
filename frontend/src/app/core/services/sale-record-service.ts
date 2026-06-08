import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { SaleBatchCreateRequest, SaleRecordSummary } from '../models/sale-record';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SaleRecordService {
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);

  saleRecordSummary = signal<SaleRecordSummary | null>(null);

  create(payload: SaleBatchCreateRequest): Observable<void> {
    return this.http.post<void>(`${this.api}/salerecord`, payload);
  }

  exists(date: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.api}/salerecord/${date}`);
  }

  getSummaryFromDateRange() {
    const numberOfDays = 10;

    return this.http.get<SaleRecordSummary>(`${this.api}/salerecord/list/${numberOfDays}`).pipe(
      tap((val) => {
        this.saleRecordSummary.set(val);
        console.log(this.saleRecordSummary());
      }),
    );
  }
}
