import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LogoutButton } from '../../shared/components/logout-button/logout-button';
import { AuthService } from '../../core/services/auth-service';
import { OrganizationService } from '../../core/services/organization-service';

@Component({
  selector: 'app-navbar',
  imports: [LogoutButton],
  templateUrl: './navbar.html',
})
export class Navbar {
  authService = inject(AuthService);
  organizationService = inject(OrganizationService);
}
