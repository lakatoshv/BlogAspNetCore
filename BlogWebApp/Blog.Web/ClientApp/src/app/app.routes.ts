import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/AuthGuard';
import { inject } from '@angular/core';

export const routes: Routes = [
    {
        path: '', redirectTo: 'blog', pathMatch: 'full'
      },
      {
        path: 'blog',
        loadChildren: () => import('./user-portal/layout.module').then(m => m.LayoutModule)
      },
      {
        path: 'admin',
        canActivateChild: [
          (route, state) => inject(AuthGuard).canActivateChild(route, state)
        ],
        loadChildren: () => import('./admin-portal/admin-portal.module').then(m => m.AdminPortalModule)
      },
      {
        path: '**',
        redirectTo: 'blog/not-found'
      }
];
