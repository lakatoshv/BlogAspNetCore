import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DefaultPagesRoutingModule } from './default-pages-routing.module';
import { IconsCardComponent } from './icons-card/icons-card.component';
import { IndexComponent } from './index/index.component';
import { AdminPostsModule } from '../admin-posts/admin-posts.module';
import { AdminCommentsModule } from '../admin-comments/admin-comments.module';


@NgModule({
  declarations: [
    IconsCardComponent,
    IndexComponent
  ],
  imports: [
    CommonModule,
    DefaultPagesRoutingModule,
    AdminPostsModule,
    AdminCommentsModule
  ]
})
export class DefaultPagesModule { }
