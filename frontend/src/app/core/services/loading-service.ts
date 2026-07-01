import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private activeRequests = 0;
  private loadingTimer: ReturnType<typeof setTimeout> | null = null;
  private wakingUpTimer: ReturnType<typeof setTimeout> | null = null;

  readonly isLoading = signal(false);
  readonly isWakingUp = signal(false);

  show() {
    // add count of request
    this.activeRequests++;

    // if previous request is still being processed, keep loading
    if (this.isLoading() || this.loadingTimer) {
      return;
    }

    // show loading if request takes longer than set time (2s) to process
    this.loadingTimer = setTimeout(() => {
      if (this.activeRequests > 0) {
        this.isLoading.set(true);
      }

      this.loadingTimer = null;
    }, 2000);

    // show waking up if request takes longer than set time (10s) to process
    this.wakingUpTimer = setTimeout(() => {
      if (this.activeRequests > 0) {
        this.isWakingUp.set(true);
      }

      this.wakingUpTimer = null;
    }, 10000);
  }

  hide() {
    // discard count of original request
    if (this.activeRequests > 0) {
      this.activeRequests--;
    }

    // if that request wasn't the last one, keep loading
    if (this.activeRequests !== 0) {
      return;
    }

    // if that request was the last one, stop loading
    if (this.loadingTimer) {
      clearTimeout(this.loadingTimer);
      this.loadingTimer = null;
    }
    if (this.wakingUpTimer) {
      clearTimeout(this.wakingUpTimer);
      this.wakingUpTimer = null;
    }
    this.isLoading.set(false);
  }
}
