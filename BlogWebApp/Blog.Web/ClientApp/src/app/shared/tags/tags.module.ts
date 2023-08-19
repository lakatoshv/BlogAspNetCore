import { TagsService } from './../../core/services/posts-services/tags.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TagsRoutingModule } from './tags-routing.module';
import { TagsListComponent } from './tags-list/tags-list.component';
import { PopularTagsComponent } from './popular-tags/popular-tags.component';
import { AddTagComponent } from './add-tag/add-tag.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    TagsListComponent,
    PopularTagsComponent,
    AddTagComponent
  ],
  imports: [
    CommonModule,
    TagsRoutingModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    TagsListComponent,
    PopularTagsComponent,
    AddTagComponent
  ],
  providers: [
    TagsService
  ]
})
export class TagsModule { }
