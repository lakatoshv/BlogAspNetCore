import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPostsListComponent } from './admin-posts-list/admin-posts-list.component';
import { ShowComponent } from 'src/app/shared/posts/show/show.component';
import { AddPostComponent } from 'src/app/shared/posts/add-post/add-post.component';
import { EditPostComponent } from 'src/app/shared/posts/edit-post/edit-post.component';

const routes: Routes = [
  {
    path: '',
    component: AdminPostsListComponent
  },
  {
    path: 'show/:post-id',
    component: ShowComponent
  },
  {
    path: 'posts/add',
    component: AddPostComponent
  },
  {
    path: 'posts/edit/:post-id',
    component: EditPostComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminPostsRoutingModule { }
