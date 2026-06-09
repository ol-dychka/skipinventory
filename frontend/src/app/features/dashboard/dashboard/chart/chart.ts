import { AfterViewInit, Component, ElementRef, Input, ViewChild } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { SaleRecordSummary, SaleRecordSummaryLine } from '../../../../core/models/sale-record';

@Component({
  selector: 'app-chart',
  template: `<canvas #myChart></canvas>`,
})
export class ChartComponent implements AfterViewInit {
  @ViewChild('myChart') chartRef!: ElementRef;
  @Input() data!: SaleRecordSummaryLine[];

  ngAfterViewInit() {
    new Chart(this.chartRef.nativeElement, {
      type: 'bar',
      data: {
        labels: this.data.map((day) => day.date),
        datasets: [{ label: 'Sales', data: this.data.map((day) => day.total) }],
      },
      options: {
        maintainAspectRatio: false,
        responsive: true,
        scales: {
          y: {
            min: Math.min(...this.data.map((day) => day.total)) - 10,
          },
        },
      },
    });
  }
}
