import { AdminTagsModule } from './../admin-tags/admin-tags.module';
import { AdminCommentsModule } from './../admin-comments/admin-comments.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DefaultPagesRoutingModule } from './default-pages-routing.module';
import { IndexComponent } from './index/index.component';
import { AdminPostsModule } from '../admin-posts/admin-posts.module';
import { IconsCardsComponent } from './icons-cards/icons-cards.component';
import { UsersModule } from '../admin-users/admin-users.module';

@NgModule({
  imports: [
    CommonModule,
    DefaultPagesRoutingModule,
    AdminPostsModule,
    AdminCommentsModule,
    UsersModule,
    AdminTagsModule
  ],
  declarations: [
    IndexComponent,
    IconsCardsComponent
  ],
  exports: [IndexComponent]
})
export class DefaultPagesModule { }
