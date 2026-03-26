import { Component, inject, signal } from '@angular/core';
import { AuthInput } from '../../../shared/components/auth-input/auth-input';
import { FormBuilder, Validators } from '@angular/forms';
import { OrganizationService } from '../../../core/services/organization-service';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-create-organization',
  imports: [AuthInput],
  templateUrl: './create-organization.html',
})
export class CreateOrganization {
  private fb = inject(FormBuilder);
  private organizationService = inject(OrganizationService);
  private router = inject(Router);

  readonly loading = signal(false);

  form = this.fb.nonNullable.group({
    name: ['', Validators.required],
  });

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    console.log(this.form.getRawValue());

    this.organizationService
      .create(this.form.getRawValue())
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(() => this.router.navigate(['/dashboard']));
  }
}
