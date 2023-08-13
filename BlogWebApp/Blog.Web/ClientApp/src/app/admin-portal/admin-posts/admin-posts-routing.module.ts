import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPostsListComponent } from './admin-posts-list/admin-posts-list.component';
import { ShowComponent } from 'src/app/shared/posts/show/show.component';

const routes: Routes = [
  {
    path: '',
    component: AdminPostsListComponent
  },
  {
    path: 'show/:post-id',
    component: ShowComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminPostsRoutingModule { }
