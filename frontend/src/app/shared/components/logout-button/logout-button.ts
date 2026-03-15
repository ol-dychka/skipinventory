import { Component, inject, signal } from '@angular/core';
import { Auth } from '../../../core/services/auth';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-logout-button',
  imports: [],
  templateUrl: './logout-button.html',
})
export class LogoutButton {
  private auth = inject(Auth);
  private router = inject(Router);

  readonly loading = signal(false);

  onClick(): void {
    this.loading.set(true);

    this.auth
      .logout()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(() => this.router.navigate(['/login']));
  }
}
