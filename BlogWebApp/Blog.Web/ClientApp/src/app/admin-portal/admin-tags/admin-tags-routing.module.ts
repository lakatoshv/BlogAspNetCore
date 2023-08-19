import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminTagsListComponent } from './admin-tags-list/admin-tags-list.component';
import { AddTagComponent } from 'src/app/shared/tags/add-tag/add-tag.component';

const routes: Routes = [
  {
    path: '',
    component: AdminTagsListComponent
  },
  {
    path: 'tags/add',
    component: AddTagComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminTagsRoutingModule { }
