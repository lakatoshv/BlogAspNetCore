import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { AdminPostsRoutingModule } from './admin-posts-routing.module';
import { PostsActivityChartComponent } from './posts-activity-chart/posts-activity-chart.component';
import { PostsModule } from 'src/app/shared/posts/posts.module';
import { PostService } from 'src/app/core/services/posts-services/post.service';
import { HttpClientService } from 'src/app/core/services/global-service/http-client-services/http-client.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { AdminPostsListComponent } from './admin-posts-list/admin-posts-list.component';
import { ChangeStatusComponent } from './admin-posts-table/change-status/change-status.component';
import { AdminPostsTableComponent } from './admin-posts-table/admin-posts-table.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@NgModule({
  declarations: [
    PostsActivityChartComponent,
    AdminPostsListComponent,
    ChangeStatusComponent,
    AdminPostsTableComponent,
  ],
  imports: [
    CommonModule,
    AdminPostsRoutingModule,
    NgxChartsModule,
    PostsModule,
    NgxPaginationModule,
    BsDropdownModule.forRoot()
  ],
  exports: [
    PostsActivityChartComponent,
    AdminPostsTableComponent
  ],
  providers: [PostService, HttpClientService]
})
export class AdminPostsModule { }
