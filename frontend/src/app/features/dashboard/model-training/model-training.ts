import { Component, inject, signal } from '@angular/core';
import { ModelTrainingService } from '../../../core/services/model-training-service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-model-training',
  imports: [],
  templateUrl: './model-training.html',
})
export class ModelTraining {
  private modelTrainingService = inject(ModelTrainingService);

  generating = signal(false);
  training = signal(false);

  generate(): void {
    this.generating.set(true);

    this.modelTrainingService
      .generate()
      .pipe(finalize(() => this.generating.set(false)))
      .subscribe();
  }

  train(): void {
    this.training.set(true);

    this.modelTrainingService
      .train()
      .pipe(finalize(() => this.training.set(false)))
      .subscribe();
  }
}
