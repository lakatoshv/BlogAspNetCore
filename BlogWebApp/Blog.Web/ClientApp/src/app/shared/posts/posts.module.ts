import { TagsModule } from './../tags/tags.module';
import { SharedModule } from './../shared.module';
import { CoreModule } from './../../core/core.module';
import { CommentsModule } from './comments/comments.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PostsRoutingModule } from './posts-routing.module';
import { PostsListComponent } from './posts-list/posts-list.component';
import { ShowComponent } from './show/show.component';
import { AddPostComponent } from './add-post/add-post.component';
import { EditPostComponent } from './edit-post/edit-post.component';
import { EditorModule } from '@tinymce/tinymce-angular';
import {NgxPaginationModule} from 'ngx-pagination';
import { MyPostsComponent } from './my-posts/my-posts.component';
import { PopularPostsComponent } from './popular-posts/popular-posts.component';
import { PostsTableComponent } from './posts-table/posts-table.component';
import { RightSidebarComponent } from './right-sidebar/right-sidebar.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  imports: [
    CommonModule,
    PostsRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    EditorModule,
    NgxPaginationModule,
    CommentsModule,
    CoreModule,
    SharedModule,
    TagsModule,
    HttpClientModule,
  ],
  declarations: [
    PostsListComponent,
    ShowComponent,
    AddPostComponent,
    EditPostComponent,
    MyPostsComponent,
    PopularPostsComponent,
    PostsTableComponent,
    RightSidebarComponent
  ],
  exports: [
    AddPostComponent,
    EditPostComponent,
    PostsTableComponent
  ]
})
export class PostsModule { }
