import { CommentsModule } from './comments/comments.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { PostsRoutingModule } from './posts-routing.module';
import { PostsListComponent } from './posts-list/posts-list.component';
import { EditorModule } from '@tinymce/tinymce-angular';
import {NgxPaginationModule} from 'ngx-pagination';
import { HttpClientModule } from '@angular/common/http';
import { PostService } from 'src/app/core/services/posts-services/post.service';
import { HttpClientService } from 'src/app/core/services/global-service/http-client-services/http-client.service';

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
  ],
  declarations: [
    PostsListComponent,
  ],
  exports: [
  ],
  providers: [PostService, HttpClientService]
})
export class PostsModule { }
