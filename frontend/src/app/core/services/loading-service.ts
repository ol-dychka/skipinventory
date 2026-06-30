import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private activeRequests = 0;
  private timer: ReturnType<typeof setTimeout> | null = null;

  readonly isLoading = signal(false);

  show() {
    this.activeRequests++;

    // already visible
    if (this.isLoading()) {
      return;
    }

    // already waiting to become visible
    if (this.timer) {
      return;
    }

    this.timer = setTimeout(() => {
      if (this.activeRequests > 0) {
        this.isLoading.set(true);
      }

      this.timer = null;
    }, 1000); // don't show for fast requests
  }

  hide() {
    if (this.activeRequests > 0) {
      this.activeRequests--;
    }

    if (this.activeRequests !== 0) {
      return;
    }

    if (this.timer) {
      clearTimeout(this.timer);
      this.timer = null;
    }

    this.isLoading.set(false);
  }
}
