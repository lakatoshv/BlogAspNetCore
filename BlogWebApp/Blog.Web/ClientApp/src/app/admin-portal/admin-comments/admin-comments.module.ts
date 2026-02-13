import { CoreModule } from './../../core/core.module';
import { CommentsModule } from './../../shared/posts/comments/comments.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminCommentsRoutingModule } from './admin-comments-routing.module';
import { CommentsActivityComponent } from './comments-activity/comments-activity.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AdminCommentsListComponent } from './admin-comments-list/admin-comments-list.component';
import { AdminCommentsTableComponent } from './admin-comments-table/admin-comments-table.component';
import { DefaultPagesModule } from '../../user-portal/default-pages/default-pages.module';

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
    CoreModule,
    DefaultPagesModule
  ],
  exports: [
    CommentsActivityComponent,
    AdminCommentsTableComponent,
    AdminCommentsListComponent
  ]
})
export class AdminCommentsModule { }
