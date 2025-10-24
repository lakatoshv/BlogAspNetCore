import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ChartOptions } from './../../../core/models/chart/ChartOptions';
import { ChartOptionsData } from './../../../core/data/chart/ChartOptionsData';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { CustomToastrService } from '../../../core/services/custom-toastr.service';
import { CommentsService } from '../../../core/services/posts-services/comments.service';

@Component({
  selector: 'app-comments-activity',
  templateUrl: './comments-activity.component.html',
  styleUrls: ['./comments-activity.component.css'],
  standalone: false
})
export class CommentsActivityComponent implements OnInit {
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
    this._commentService.commentsActivity().subscribe(
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
   * @param _commentService CommentService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _commentService: CommentsService,
    private _customToastrService: CustomToastrService) {
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
