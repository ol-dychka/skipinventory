import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth-service';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-logout-button',
  imports: [],
  templateUrl: './logout-button.html',
})
export class LogoutButton {
  authService = inject(AuthService);
  router = inject(Router);

  readonly loading = signal(false);

  onClick(): void {
    this.loading.set(true);

    this.authService
      .logout()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(() => this.router.navigate(['/login']));
  }
}
