import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { LogoutButton } from '../../shared/components/logout-button/logout-button';
import { AuthService } from '../../core/services/auth-service';
import { OrganizationService } from '../../core/services/organization-service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-navbar',
  imports: [LogoutButton],
  templateUrl: './navbar.html',
})
export class Navbar {
  authService = inject(AuthService);
  organizationService = inject(OrganizationService);
  router = inject(Router);

  readonly loading = signal(false);

  handleOrganizationLogout() {
    this.loading.set(true);

    this.organizationService
      .exit()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(() => this.router.navigate(['/choose-organization']));
  }
}
