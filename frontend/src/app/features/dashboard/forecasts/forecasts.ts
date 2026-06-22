import { Component, inject, signal } from '@angular/core';
import { ModelTrainingService } from '../../../core/services/model-training-service';
import { finalize } from 'rxjs';
import { displayDate } from '../../../core/helpers/display-date';

@Component({
  selector: 'app-forecasts',
  imports: [],
  templateUrl: './forecasts.html',
})
export class Forecasts {
  modelTrainingService = inject(ModelTrainingService);

  forecasting = signal(false);

  formatDate(date: Date) {
    return displayDate(date);
  }

  forecast(): void {
    this.forecasting.set(true);

    this.modelTrainingService
      .forecast()
      .pipe(finalize(() => this.forecasting.set(false)))
      .subscribe();
  }
}
