import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminCommentsRoutingModule } from './admin-comments-routing.module';
import { CommentsActivityChartComponent } from './comments-activity-chart/comments-activity-chart.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { CommentService } from 'src/app/core/services/posts-services/comment.service';
import { AdminCommentsTableComponent } from './admin-comments-table/admin-comments-table.component';


@NgModule({
  declarations: [
    CommentsActivityChartComponent,
    AdminCommentsTableComponent
  ],
  imports: [
    CommonModule,
    AdminCommentsRoutingModule,
    NgxChartsModule
  ],
  exports: [
    CommentsActivityChartComponent,
    AdminCommentsTableComponent
  ],
  providers: [CommentService]
})
export class AdminCommentsModule { }
