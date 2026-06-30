import { Component, inject } from '@angular/core';
import { LoadingService } from '../../core/services/loading-service';

@Component({
  selector: 'app-loading-overlay',
  imports: [],
  templateUrl: './loading-overlay.html',
})
export class LoadingOverlay {
  loadingService = inject(LoadingService);
}
