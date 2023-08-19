import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminTagsListComponent } from './admin-tags-list/admin-tags-list.component';

const routes: Routes = [
  {
    path: '',
    component: AdminTagsListComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminTagsRoutingModule { }
