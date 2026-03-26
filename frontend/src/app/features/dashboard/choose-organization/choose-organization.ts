import { Component, inject } from '@angular/core';
import { OrganizationService } from '../../../core/services/organization-service';
import { AuthService } from '../../../core/services/auth-service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-choose-organization',
  imports: [RouterLink],
  templateUrl: './choose-organization.html',
})
export class ChooseOrganization {
  organizationService = inject(OrganizationService);
  authService = inject(AuthService);
}
