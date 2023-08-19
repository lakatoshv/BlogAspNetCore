import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminTagsListComponent } from './admin-tags-list/admin-tags-list.component';
import { AddTagComponent } from 'src/app/shared/tags/add-tag/add-tag.component';
import { EditTagComponent } from 'src/app/shared/tags/edit-tag/edit-tag.component';

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
    path: 'tags/edit/:post-id',
    component: EditTagComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminTagsRoutingModule { }
