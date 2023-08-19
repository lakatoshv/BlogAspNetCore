import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminTagsRoutingModule } from './admin-tags-routing.module';
import { TagsActivityChartComponent } from './tags-activity-chart/tags-activity-chart.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AdminTagsListComponent } from './admin-tags-list/admin-tags-list.component';
import { AdminTagsTableComponent } from './admin-tags-table/admin-tags-table.component';
import { TagsModule } from 'src/app/shared/tags/tags.module';


@NgModule({
  declarations: [
    TagsActivityChartComponent,
    AdminTagsListComponent,
    AdminTagsTableComponent
  ],
  imports: [
    CommonModule,
    AdminTagsRoutingModule,
    NgxChartsModule,
    TagsModule
  ],
  exports: [
    TagsActivityChartComponent, 
    AdminTagsTableComponent]
})
export class AdminTagsModule { }
