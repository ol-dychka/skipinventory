import { Component, inject } from '@angular/core';
import { OrganizationService } from '../../../core/services/organization-service';

@Component({
  selector: 'app-members',
  imports: [],
  templateUrl: './members.html',
})
export class Members {
  organizationService = inject(OrganizationService);

  onPromote(id: string) {
    this.organizationService.promote(id).subscribe();
  }

  onDemote(id: string) {
    this.organizationService.demote(id).subscribe();
  }
}
