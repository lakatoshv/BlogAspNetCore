import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminCommentsListComponent } from './admin-comments-list/admin-comments-list.component';

const routes: Routes = [
  {
    path: '',
    component: AdminCommentsListComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminCommentsRoutingModule { }
