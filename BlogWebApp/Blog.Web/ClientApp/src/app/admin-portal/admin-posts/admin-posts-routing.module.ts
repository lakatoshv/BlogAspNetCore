import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPostsListComponent } from './admin-posts-list/admin-posts-list.component';

const routes: Routes = [
  {
    path: '',
    component: AdminPostsListComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminPostsRoutingModule { }
