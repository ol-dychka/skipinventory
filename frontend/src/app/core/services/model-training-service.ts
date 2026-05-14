import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ModelTrainingService {
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);

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
}
