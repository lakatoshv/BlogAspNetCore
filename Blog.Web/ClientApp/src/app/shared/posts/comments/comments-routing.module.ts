import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {CommentsListComponent} from './comments-list/comments-list.component';
import { from } from 'rxjs';
import { EditCommentComponent } from './edit-comment/edit-comment.component';
import { AddCommentComponent } from './add-comment/add-comment.component';

const routes: Routes = [
  {
    path: '',
    component: CommentsListComponent
  },
  {
    path: 'add',
    component: AddCommentComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CommentsRoutingModule { }
