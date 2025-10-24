import { EditTagComponent } from './../../shared/tags/edit-tag/edit-tag.component';
import { AddTagComponent } from './../../shared/tags/add-tag/add-tag.component';
import { AdminTagsListComponent } from './admin-tags-list/admin-tags-list.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    component: AdminTagsListComponent
  },
  {
    path: 'tags/add',
    component: AddTagComponent
  },
  {
    path: 'tags/edit/:postId',
    component: EditTagComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminTagsRoutingModule { }
