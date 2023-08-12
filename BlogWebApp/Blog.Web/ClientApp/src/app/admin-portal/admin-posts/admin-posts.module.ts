import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { AdminPostsRoutingModule } from './admin-posts-routing.module';
import { PostsActivityChartComponent } from './posts-activity-chart/posts-activity-chart.component';
import { PostService } from 'src/app/core/services/posts-services/post.service';
import { HttpClientService } from 'src/app/core/services/global-service/http-client-services/http-client.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { ChangeStatusComponent } from './admin-posts-table/change-status/change-status.component';
import { AdminPostsTableComponent } from './admin-posts-table/admin-posts-table.component';


@NgModule({
  declarations: [
    PostsActivityChartComponent,
    ChangeStatusComponent,
    AdminPostsTableComponent,
  ],
  imports: [
    CommonModule,
    AdminPostsRoutingModule,
    NgxChartsModule,
    NgxPaginationModule
  ],
  exports: [
    PostsActivityChartComponent,
    AdminPostsTableComponent
  ],
  providers: [PostService, HttpClientService]
})
export class AdminPostsModule { }
