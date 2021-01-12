import { AuthorizationComponent } from './user-portal/user/authorization/authorization.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '', redirectTo: 'blog', pathMatch: 'full'
  },
  {
    path: 'blog',
    loadChildren: './user-portal/layout.module#LayoutModule'
  },
  {
    path: '**',
    redirectTo: 'blog/not-found'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
