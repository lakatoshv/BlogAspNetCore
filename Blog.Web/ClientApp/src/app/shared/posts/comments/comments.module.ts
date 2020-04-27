import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { CommentsRoutingModule } from './comments-routing.module';
import { CommentsListComponent } from './comments-list/comments-list.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { EditCommentComponent } from './edit-comment/edit-comment.component';
import { AddCommentComponent } from './add-comment/add-comment.component';

@NgModule({
  imports: [
    CommonModule,
    CommentsRoutingModule,
    ReactiveFormsModule,
    NgxPaginationModule
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
