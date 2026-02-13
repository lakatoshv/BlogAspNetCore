import { Component, OnInit, ViewChild, ElementRef, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { ChartOptions } from './../../../core/models/chart/ChartOptions';
import { ChartOptionsData } from './../../../core/data/chart/ChartOptionsData';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { CustomToastrService } from '../../../core/services/custom-toastr.service';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-users-activity-chart',
  templateUrl: './users-activity-chart.component.html',
  styleUrls: ['./users-activity-chart.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UsersActivityChartComponent implements OnInit {
  /**
   * @param pbgChartContainer ElementRef
   */
  @ViewChild('pbgChartContainer') pbgChartContainer: ElementRef | undefined;

  /**
   * @param chartOptions ChartOptions
   */
  chartOptions: ChartOptions = ChartOptionsData;

  /**
   * @param isLoaded boolean
   */
  isLoaded: boolean = false;

  /**
   * @inheritdoc
   */
  async ngOnInit(): Promise<void> {
    this._usersService.usersActivity()
     .pipe(
       finalize(() => {
         this.isLoaded = true;
         this._changeDetectorRef.markForCheck();
       })
     )
    .subscribe({
      next: (response: any) => {
        this.chartOptions.Data[0] = response;
        this.chartOptions = this.chartOptions;
      },
      error: (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
      }
    });
  }

  /**
   * @param _usersService UsersService
   * @param _customToastrService CustomToastrService
   * @param _changeDetectorRef: ChangeDetectorRef
   */
  constructor(
    private _usersService: UsersService,
    private _customToastrService: CustomToastrService,
    private _changeDetectorRef: ChangeDetectorRef
  ) {
  }

  /**
   * Select chart item event.
   * @param data any
   */
  onSelect(data: any): void {
    console.log('Item clicked', JSON.parse(JSON.stringify(data)));
  }

  /**
   * Activate chart item event.
   * @param data any
   */
  onActivate(data: any): void {
    console.log('Activate', JSON.parse(JSON.stringify(data)));
  }

  /**
   * Deactivate chart item event.
   * @param data any
   */
  onDeactivate(data: any): void {
    console.log('Deactivate', JSON.parse(JSON.stringify(data)));
  }
}
