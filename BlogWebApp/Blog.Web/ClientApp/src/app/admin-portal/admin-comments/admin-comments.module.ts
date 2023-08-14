import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminCommentsRoutingModule } from './admin-comments-routing.module';
import { CommentsActivityChartComponent } from './comments-activity-chart/comments-activity-chart.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';


@NgModule({
  declarations: [
    CommentsActivityChartComponent
  ],
  imports: [
    CommonModule,
    AdminCommentsRoutingModule,
    NgxChartsModule
  ],
  exports: [
    CommentsActivityChartComponent
  ]
})
export class AdminCommentsModule { }
