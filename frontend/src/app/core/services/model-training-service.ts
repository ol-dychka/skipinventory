import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Forecast } from '../models/forecast';

@Injectable({
  providedIn: 'root',
})
export class ModelTrainingService {
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);

  forecasts = signal<Forecast[]>([]);

  generate(): Observable<void> {
    return this.http
      .post<void>(`${this.api}/mlservice/train/generate`, {})
      .pipe(tap(() => console.log('generating training data is done')));
  }

  train(): Observable<void> {
    return this.http
      .post<void>(`${this.api}/mlservice/train/data`, {})
      .pipe(tap(() => console.log('training is done')));
  }

  forecast() {
    return this.http
      .post<any>(`${this.api}/mlservice/forecast`, {})
      .pipe(tap((val) => console.log(val)));
  }

  list() {
    return this.http.get<Forecast[]>(`${this.api}/mlservice/list`).pipe(
      tap((list) => {
        this.forecasts.set(list);
        console.log(this.forecasts());
      }),
    );
  }
}
