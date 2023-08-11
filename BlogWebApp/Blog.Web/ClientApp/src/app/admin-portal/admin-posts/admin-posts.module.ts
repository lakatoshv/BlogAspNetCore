import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { AdminPostsRoutingModule } from './admin-posts-routing.module';
import { PostsActivityChartComponent } from './posts-activity-chart/posts-activity-chart.component';
import { PostService } from 'src/app/core/services/posts-services/post.service';
import { HttpClientService } from 'src/app/core/services/global-service/http-client-services/http-client.service';
import { PostsTableComponent } from './posts-table/posts-table.component';
import { ChangeStatusComponent } from './posts-table/change-status/change-status.component';
import { NgxPaginationModule } from 'ngx-pagination';


@NgModule({
  declarations: [
    PostsActivityChartComponent,
    PostsTableComponent,
    ChangeStatusComponent
  ],
  imports: [
    CommonModule,
    AdminPostsRoutingModule,
    NgxChartsModule,
    NgxPaginationModule
  ],
  exports: [
    PostsActivityChartComponent,
    PostsTableComponent
  ],
  providers: [PostService, HttpClientService]
})
export class AdminPostsModule { }
