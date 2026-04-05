import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { OrganizationService } from '../../../core/services/organization-service';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-join-organization',
  imports: [ReactiveFormsModule],
  templateUrl: './join-organization.html',
})
export class JoinOrganization implements OnInit {
  private fb = inject(FormBuilder);
  organizationService = inject(OrganizationService);
  private router = inject(Router);

  chosenOrgId = signal<string | null>(null);
  readonly loadingSubmit = signal(false);
  readonly loadingList = signal(false);

  ngOnInit(): void {
    if (this.organizationService.availableOrganizations.length === 0) {
      this.loadingList.set(true);
      this.organizationService
        .getPreviews()
        .pipe(finalize(() => this.loadingList.set(false)))
        .subscribe();
    }
  }

  onSubmit(): void {
    if (this.chosenOrgId()) {
      console.log('submit');

      this.loadingSubmit.set(true);

      this.organizationService
        .request(this.chosenOrgId()!)
        .pipe(finalize(() => this.loadingSubmit.set(false)))
        .subscribe((organizationId) => this.router.navigate(['/choose-organization']));
    }
  }
}
