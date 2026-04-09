import { Component, inject } from '@angular/core';
import { OrganizationService } from '../../../core/services/organization-service';

@Component({
  selector: 'app-join-requests',
  imports: [],
  templateUrl: './join-requests.html',
})
export class JoinRequests {
  organizationService = inject(OrganizationService);

  onAccept(id: string) {
    this.organizationService.resolveRequest(id, true).subscribe();
  }

  onDeny(id: string) {
    this.organizationService.resolveRequest(id, false).subscribe();
  }
}
