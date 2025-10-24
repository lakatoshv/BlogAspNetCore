import { CoreModule } from './../../core/core.module';
import { CommentsModule } from './../../shared/posts/comments/comments.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminCommentsRoutingModule } from './admin-comments-routing.module';
import { CommentsActivityComponent } from './comments-activity/comments-activity.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AdminCommentsListComponent } from './admin-comments-list/admin-comments-list.component';
import { AdminCommentsTableComponent } from './admin-comments-table/admin-comments-table.component';

@NgModule({
  declarations: [
    CommentsActivityComponent,
    AdminCommentsTableComponent,
    AdminCommentsListComponent
  ],
  imports: [
    CommonModule,
    AdminCommentsRoutingModule,
    NgxChartsModule,
    CommentsModule,
    CoreModule
  ],
  exports: [
    CommentsActivityComponent,
    AdminCommentsTableComponent,
    AdminCommentsListComponent
  ]
})
export class AdminCommentsModule { }
