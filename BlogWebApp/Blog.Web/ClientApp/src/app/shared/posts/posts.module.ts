import { TagsModule } from './../tags/tags.module';
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
import { HttpClientModule } from '@angular/common/http';
import { PostService } from 'src/app/core/services/posts-services/post.service';
import { CommentService } from 'src/app/core/services/posts-services/comment.service';
import { HttpClientService } from 'src/app/core/services/global-service/http-client-services/http-client.service';
import { PostsTableComponent } from './posts-table/posts-table.component';
import { RightSidebarComponent } from './right-sidebar/right-sidebar.component';
import { PopularPostsComponent } from './popular-posts/popular-posts.component';

@NgModule({
  imports: [
    CommonModule,
    PostsRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    EditorModule,
    NgxPaginationModule,
    CommentsModule,
    HttpClientModule,
    TagsModule
  ],
  declarations: [
    PostsListComponent,
    ShowComponent,
    AddPostComponent,
    EditPostComponent,
    MyPostsComponent,
    PostsTableComponent,
    RightSidebarComponent,
    PopularPostsComponent
  ],
  exports: [
    AddPostComponent,
    EditPostComponent,
    PostsTableComponent
  ],
  providers: [PostService, CommentService, HttpClientService]
})
export class PostsModule { }
