import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPostsListComponent } from './admin-posts-list/admin-posts-list.component';
import { ShowComponent } from 'src/app/shared/posts/show/show.component';
import { AddPostComponent } from 'src/app/shared/posts/add-post/add-post.component';

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
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminPostsRoutingModule { }
