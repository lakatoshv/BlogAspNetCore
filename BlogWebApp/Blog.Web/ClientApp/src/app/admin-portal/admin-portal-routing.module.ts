import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../core/guards/AuthGuard';
import { LayoutComponentComponent } from './layout-component/layout-component.component';
import { DefaultPagesModule } from './default-pages/default-pages.module';
import { NotFoundComponent } from '../shared/errors/not-found/not-found.component';
import { AdminPostsModule } from './admin-posts/admin-posts.module';
import { AdminCommentsModule } from './admin-comments/admin-comments.module';
import { AdminTagsModule } from './admin-tags/admin-tags.module';

const routes: Routes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    component: LayoutComponentComponent,
    children: [
      {
        path: '',
        loadChildren: () => DefaultPagesModule
      },
      {
        path: 'posts',
        loadChildren: () => AdminPostsModule
      },
      {
        path: 'comments',
        loadChildren: () => AdminCommentsModule
      },
      {
        path: 'tags',
        loadChildren: () => AdminTagsModule
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
