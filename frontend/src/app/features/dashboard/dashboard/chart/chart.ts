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

  private getAppendedData(): SaleRecordSummaryLine[] {
    const result = [
      ...this.data.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()),
    ];
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const carrier =
      result.length > 0
        ? new Date(result[result.length - 1].date)
        : new Date(today.getTime() - 9 * 24 * 60 * 60 * 1000);

    carrier.setHours(0, 0, 0, 0);

    while (carrier < today && result.length < 10) {
      carrier.setDate(carrier.getDate() + 1);
      result.push({
        date: new Date(carrier),
        total: 0,
      });
    }

    return result;
  }

  private displayDate(date: Date) {
    date = new Date(date);
    return date.toISOString().split('T')[0];
  }

  ngAfterViewInit() {
    const appendedData = this.getAppendedData();

    new Chart(this.chartRef.nativeElement, {
      type: 'bar',
      data: {
        labels: appendedData.map((day) => this.displayDate(day.date)),
        datasets: [
          {
            label: 'Sales',
            data: appendedData.map((day) => day.total),
            backgroundColor: '#778da9',
          },
        ],
      },
      options: {
        maintainAspectRatio: false,
        responsive: true,
        scales: {
          y: {
            min: Math.max(Math.min(...appendedData.map((day) => day.total)) - 10, 0),
          },
        },
        color: 'primary',
      },
    });
  }
}
