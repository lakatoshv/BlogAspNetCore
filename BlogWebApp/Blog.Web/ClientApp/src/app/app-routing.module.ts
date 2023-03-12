import { AuthorizationComponent } from './user-portal/user/authorization/authorization.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guards/AuthGuard';
import { LayoutModule } from './user-portal/layout.module';
import { AdminPortalModule } from './admin-portal/admin-portal.module';

const routes: Routes = [
  {
    path: '', redirectTo: 'blog', pathMatch: 'full'
  },
  {
    path: 'blog',
    loadChildren: () => LayoutModule
  },
  {
    path: 'admin',
    canActivateChild: [AuthGuard],
    loadChildren: () => AdminPortalModule
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
