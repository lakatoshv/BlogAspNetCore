import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { AdminPostsRoutingModule } from './admin-posts-routing.module';
import { PostsActivityChartComponent } from './posts-activity-chart/posts-activity-chart.component';


@NgModule({
  declarations: [
    PostsActivityChartComponent
  ],
  imports: [
    CommonModule,
    AdminPostsRoutingModule,
    NgxChartsModule
  ],
  exports: [
    PostsActivityChartComponent
  ]
})
export class AdminPostsModule { }
