import { Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthInput } from '../../../shared/components/auth-input/auth-input';
import { Auth } from '../../../core/services/auth';
import { finalize } from 'rxjs';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, AuthInput, RouterLink],
  templateUrl: './login.html',
})
export class Login {
  private fb = inject(FormBuilder);
  private auth = inject(Auth);
  private router = inject(Router);

  readonly loading = signal(false);

  form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  get emailError(): string | null {
    const ctrl = this.form.get('email');
    if (!ctrl?.touched || !ctrl.errors) return null;
    if (ctrl.errors['required']) return 'Email is required';
    if (ctrl.errors['email']) return 'Enter a valid email address';
    return null;
  }

  get passwordError(): string | null {
    const ctrl = this.form.get('password');
    if (!ctrl?.touched || !ctrl.errors) return null;
    if (ctrl.errors['required']) return 'Password is required';
    if (ctrl.errors['minlength']) return 'Password must be at least 8 characters';
    return null;
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    this.auth
      .login(this.form.getRawValue())
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(() => this.router.navigate(['/dashboard']));
  }
}
