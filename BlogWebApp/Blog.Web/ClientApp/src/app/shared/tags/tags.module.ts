import { TagsService } from './../../core/services/posts-services/tags.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TagsRoutingModule } from './tags-routing.module';
import { AddTagComponent } from './add-tag/add-tag.component';
import { EditTagComponent } from './edit-tag/edit-tag.component';
import { TagsListComponent } from './tags-list/tags-list.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PopularTagsComponent } from './popular-tags/popular-tags.component';

@NgModule({
  declarations: [
    AddTagComponent, 
    EditTagComponent, 
    TagsListComponent, 
    PopularTagsComponent
  ],
  imports: [
    CommonModule,
    TagsRoutingModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [TagsService],
  exports: [
    AddTagComponent, 
    EditTagComponent, 
    TagsListComponent,
    PopularTagsComponent
  ]
})
export class TagsModule { }
