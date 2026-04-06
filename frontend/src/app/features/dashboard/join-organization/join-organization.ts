import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { OrganizationService } from '../../../core/services/organization-service';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { OrganizationPreviewModel } from '../../../core/models/organization';

@Component({
  selector: 'app-join-organization',
  imports: [ReactiveFormsModule],
  templateUrl: './join-organization.html',
})
export class JoinOrganization implements OnInit {
  private fb = inject(FormBuilder);
  organizationService = inject(OrganizationService);
  private router = inject(Router);

  readonly filter = signal('');
  readonly chosenOrganization = signal<OrganizationPreviewModel | null>(null);
  readonly visibleOrganizations = signal<OrganizationPreviewModel[]>([]);
  readonly loadingSubmit = signal(false);
  readonly loadingList = signal(false);

  ngOnInit(): void {
    if (this.organizationService.availableOrganizations().length === 0) {
      this.loadingList.set(true);
      this.organizationService
        .getPreviews()
        .pipe(
          finalize(() => {
            this.loadingList.set(false);
            this.visibleOrganizations.set(this.getOrganizations());
          }),
        )
        .subscribe();
    }
  }

  getOrganizations(): OrganizationPreviewModel[] {
    var orgs = this.organizationService.availableOrganizations();
    var filtered = this.filter()
      ? orgs.filter((o) => o.name.includes(this.filter())).slice(0, 3)
      : orgs.slice(0, 3);
    return filtered;
  }

  onInput(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.filter.set(value);
    this.visibleOrganizations.set(this.getOrganizations());
  }

  onSubmit(): void {
    if (this.chosenOrganization()) {
      console.log('submit');

      this.loadingSubmit.set(true);

      this.organizationService
        .request(this.chosenOrganization()!.id)
        .pipe(finalize(() => this.loadingSubmit.set(false)))
        .subscribe((organizationId) => this.router.navigate(['/choose-organization']));
    }
  }
}
