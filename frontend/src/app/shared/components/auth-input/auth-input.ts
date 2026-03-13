import { Component, input, output, signal, computed, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';

export type InputType = 'email' | 'password' | 'text';

@Component({
  selector: 'app-auth-input',
  imports: [ReactiveFormsModule],
  templateUrl: './auth-input.html',
  styleUrl: './auth-input.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AuthInput),
      multi: true,
    },
  ],
})
export class AuthInput implements ControlValueAccessor {
  // Inputs
  type = input.required<InputType>();
  label = input.required<string>();
  placeholder = input<string>('');
  error = input<string | null>(null);
  disabled = input<boolean>(false);

  // Internal state
  value = signal<string>('');
  isFocused = signal<boolean>(false);
  isPasswordVisible = signal<boolean>(false);

  // Computed
  resolvedType = computed<string>(() => {
    if (this.type() === 'password') {
      return this.isPasswordVisible() ? 'text' : 'password';
    }
    return this.type();
  });

  hasValue = computed(() => this.value().length > 0);
  isPassword = computed(() => this.type() === 'password');

  // CVA callbacks
  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.value.set(value ?? '');
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    // handled via input signal + attr.disabled binding in template
  }

  onInput(event: Event): void {
    const val = (event.target as HTMLInputElement).value;
    this.value.set(val);
    this.onChange(val);
  }

  onFocus(): void {
    this.isFocused.set(true);
  }

  onBlur(): void {
    this.isFocused.set(false);
    this.onTouched();
  }

  togglePasswordVisibility(): void {
    this.isPasswordVisible.update((v) => !v);
  }
}
