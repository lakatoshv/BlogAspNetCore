import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PostsListComponent } from './posts-list/posts-list.component';
import { ShowComponent } from './show/show.component';
import { AddPostComponent } from './add-post/add-post.component';
import { ReactiveFormsModule } from '@angular/forms';
import { EditPostComponent } from './edit-post/edit-post.component';
import { MyPostsComponent } from './my-posts/my-posts.component';

const routes: Routes = [
  {
    path: '',
    component: PostsListComponent
  },
  {
    path: 'post/:post-id',
    component: ShowComponent
  },
  {
    path: 'posts/add',
    component: AddPostComponent
  },
  {
    path: 'post/edit/:post-id',
    component: EditPostComponent
  },
  {
    path: 'my-posts',
    component: MyPostsComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes), ReactiveFormsModule],
  exports: [RouterModule]
})
export class PostsRoutingModule { }
