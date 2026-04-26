import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  forwardRef,
  Input,
  Output,
  signal,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-scaled-input',
  imports: [],
  templateUrl: './scaled-input.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => ScaledInput), multi: true },
  ],
})
export class ScaledInput implements ControlValueAccessor {
  Math = Math;
  @Input() label = '';
  @Input() id = '';
  @Input() type: 'text' | 'number' = 'text';
  @Input() disabled = false;

  value = signal<string | number>('');
  @Output() valueChange = new EventEmitter<string | number | undefined>();

  private onChange: (val: string | number) => void = () => {};
  private onTouched: () => void = () => {};

  // Called by Angular when the form value changes programmatically
  // e.g. form.patchValue(...) or form.setValue(...)
  writeValue(val: string | number): void {
    this.value.set(val ?? '');
  }

  // Angular gives us this function to call whenever our value changes
  registerOnChange(fn: (val: string | number) => void): void {
    this.onChange = fn;
  }

  // Angular gives us this function to call when the control is "touched"
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  // Called by Angular when the form control is disabled/enabled
  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onInput(e: Event) {
    const raw = (e.target as HTMLInputElement).value;

    let val: string | number = raw;
    if (this.type === 'number') val = Number(raw);

    this.value.set(val);

    this.onChange(val); // 👈 notify the form control
    this.onTouched(); // 👈 mark as touched on input
    this.valueChange.emit(val); // still works for standalone usage
  }

  onBlur() {
    this.onTouched(); // 👈 also mark touched on blur (conventional)
  }
}
