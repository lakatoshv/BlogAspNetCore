import { PostsModule } from './../../shared/posts/posts.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminPostsRoutingModule } from './admin-posts-routing.module';
import { AdminPostsListComponent } from './admin-posts-list/admin-posts-list.component';
import { AdminPostsTableComponent } from './admin-posts-table/admin-posts-table.component';
import { ChangeStatusComponent } from './admin-posts-table/change-status/change-status.component';
import { PostsActivityChartComponent } from './posts-activity-chart/posts-activity-chart.component';

@NgModule({
  imports: [
    CommonModule,
    AdminPostsRoutingModule,
    NgxChartsModule,
    PostsModule
  ],
  declarations: [
    AdminPostsTableComponent,
    PostsActivityChartComponent,
    AdminPostsListComponent,
    ChangeStatusComponent,
  ],
  exports: [
    AdminPostsTableComponent,
    PostsActivityChartComponent
  ]
})
export class AdminPostsModule { }
