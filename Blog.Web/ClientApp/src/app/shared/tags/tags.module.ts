import { TagsService } from './../../core/services/posts-services/tags.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TagsRoutingModule } from './tags-routing.module';
import { PopularTagsComponent } from './popular-tags/popular-tags.component';

@NgModule({
  declarations: [
    PopularTagsComponent
  ],
  imports: [
    CommonModule,
    TagsRoutingModule
  ],
  exports: [
    PopularTagsComponent
  ],
  providers: [
    TagsService
  ]
})
export class TagsModule { }
