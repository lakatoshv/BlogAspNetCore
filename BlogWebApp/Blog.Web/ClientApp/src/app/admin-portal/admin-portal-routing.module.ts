import { NgModule } from '@angular/core';
import { LayoutComponentComponent } from './layout-component/layout-component.component';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from '../shared/errors/not-found/not-found.component';
import { AuthGuard } from '../core/guards/AuthGuard';

const routes: Routes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    component: LayoutComponentComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./default-pages/default-pages.module').then(m => m.DefaultPagesModule)
      },
      {
        path: 'posts',
        loadChildren: () => import('./admin-posts/admin-posts.module').then(m => m.AdminPostsModule)
      },
      {
        path: 'comments',
        loadChildren: () => import('./admin-comments/admin-comments.module').then(m => m.AdminCommentsModule)
      },
      {
        path: 'comments/:postId',
        loadChildren: () => import('./admin-comments/admin-comments.module').then(m => m.AdminCommentsModule)
      },
      {
        path: 'tags',
        loadChildren: () => import('./admin-tags/admin-tags.module').then(m => m.AdminTagsModule)
      },
      {
        path: 'not-found',
        component: NotFoundComponent
      },
      {
        path: '**',
        component: NotFoundComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminPortalRoutingModule { }
