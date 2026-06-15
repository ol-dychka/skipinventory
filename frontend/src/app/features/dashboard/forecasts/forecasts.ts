import { Component, inject, signal } from '@angular/core';
import { ModelTrainingService } from '../../../core/services/model-training-service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-forecasts',
  imports: [],
  templateUrl: './forecasts.html',
})
export class Forecasts {
  private modelTrainingService = inject(ModelTrainingService);

  forecasting = signal(false);

  forecast(): void {
    this.forecasting.set(true);

    this.modelTrainingService
      .forecast()
      .pipe(finalize(() => this.forecasting.set(false)))
      .subscribe();
  }
}
