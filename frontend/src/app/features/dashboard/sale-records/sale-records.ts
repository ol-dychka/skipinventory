import { Component, signal, inject, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { switchMap, tap, catchError, filter } from 'rxjs/operators';
import { of } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';
import { SaleRecordService } from '../../../core/services/sale-record-service';
import { CreateSaleRecord } from '../create-sale-record/create-sale-record';

@Component({
  selector: 'app-sale-records',
  imports: [ReactiveFormsModule, CreateSaleRecord],
  templateUrl: './sale-records.html',
})
export class SaleRecords implements OnInit {
  private saleRecordService = inject(SaleRecordService);

  selectedDate = new FormControl<string>('');
  exists = signal<boolean>(false);
  loading = signal(false);
  error = signal<string | null>(null);

  ngOnInit() {
    this.selectedDate.valueChanges
      .pipe(
        filter((date) => !!date),
        tap(() => {
          console.log('nigga', this.selectedDate);
          this.loading.set(true);
          this.error.set(null);
        }),
        switchMap(
          (
            date, // cancels previous request on new date
          ) =>
            this.saleRecordService.exists(date!).pipe(
              catchError((err) => {
                this.error.set('Failed to load data.');
                return of(null);
              }),
            ),
        ),
        tap(() => this.loading.set(false)),
      )
      .subscribe((result) => {
        console.log(result);
        this.exists.set(result ? true : false);
      });
  }
}
