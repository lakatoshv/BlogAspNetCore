import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponentComponent } from './layout-component/layout-component.component';
import { AboutComponent } from './default-pages/about/about.component';
import { ContactsComponent } from './default-pages/contacts/contacts.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponentComponent,
    children: [
      /*{
        path: '',
        loadChildren: '../shared/posts/posts.module#PostsModule'
      },*/
      {
        path: '',
        component: AboutComponent
      },
      {
        path: 'contacts',
        component: ContactsComponent
      },
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
