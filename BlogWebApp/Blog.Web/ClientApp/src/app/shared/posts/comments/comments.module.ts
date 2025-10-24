import { CoreModule } from './../../../core/core.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { CommentsRoutingModule } from './comments-routing.module';
import { CommentsListComponent } from './comments-list/comments-list.component';
import { AddCommentComponent } from './add-comment/add-comment.component';
import { EditCommentComponent } from './edit-comment/edit-comment.component';
import { NgxPaginationModule } from 'ngx-pagination';

@NgModule({
  imports: [
    CommonModule,
    CommentsRoutingModule,
    ReactiveFormsModule,
    NgxPaginationModule,
    CoreModule
  ],
  exports: [
    CommentsListComponent,
    AddCommentComponent,
    EditCommentComponent
  ],
  declarations: [
    CommentsListComponent,
    AddCommentComponent,
    EditCommentComponent
  ]
})
export class CommentsModule { }
