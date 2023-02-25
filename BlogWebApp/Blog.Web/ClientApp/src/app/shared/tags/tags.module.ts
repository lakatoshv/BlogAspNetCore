import { TagsService } from './../../core/services/posts-services/tags.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TagsRoutingModule } from './tags-routing.module';
import { TagsListComponent } from './tags-list/tags-list.component';
import { PopularTagsComponent } from './popular-tags/popular-tags.component';

@NgModule({
  declarations: [
    TagsListComponent,
    PopularTagsComponent
  ],
  imports: [
    CommonModule,
    TagsRoutingModule
  ],
  exports: [
    TagsListComponent,
    PopularTagsComponent
  ],
  providers: [
    TagsService
  ]
})
export class TagsModule { }
