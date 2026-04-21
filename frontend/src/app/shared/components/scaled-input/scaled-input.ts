import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-scaled-input',
  imports: [],
  templateUrl: './scaled-input.html',
})
export class ScaledInput {
  Math = Math;
  @Input() label = '';
  @Input() value?: string | number;
  @Input() id = '';
  @Input() disabled = false;
  @Input() type: 'text' | 'number' = 'text';

  @Output() valueChange = new EventEmitter<string | number | undefined>();

  onInput(e: Event) {
    const raw = (e.target as HTMLInputElement).value;

    let val: string | number | undefined = raw;
    if (this.type === 'number') {
      val = raw === '' ? undefined : Number(raw);
    } else {
      val = raw || undefined;
    }

    this.value = val;
    this.valueChange.emit(val);
  }
}
