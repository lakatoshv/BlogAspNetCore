import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ChartOptions } from './../../../core/models/chart/ChartOptions';
import { ChartOptionsData } from './../../../core/data/chart/ChartOptionsData';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { CustomToastrService } from '../../../core/services/custom-toastr.service';
import { TagsService } from '../../../core/services/posts-services/tags.service';

@Component({
  selector: 'app-tags-activity-chart',
  templateUrl: './tags-activity-chart.component.html',
  styleUrls: ['./tags-activity-chart.component.css'],
  standalone: false
})
export class TagsActivityChartComponent implements OnInit {
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
  ngOnInit(): void {
    this._tagsService.tagsActivity().subscribe(
      (response: any) => {
        this.chartOptions.Data[0] = response;
        this.chartOptions = this.chartOptions;
        this.isLoaded = true;
      },
      (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
      });
  }

  /**
   * @param _tagsService UsersService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _tagsService: TagsService,
    private _customToastrService: CustomToastrService
  ) {
  }

  /**
   * Select chart item event.
   * 
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
