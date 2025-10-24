import { TagsModule } from './../../shared/tags/tags.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminTagsRoutingModule } from './admin-tags-routing.module';
import { TagsActivityChartComponent } from './tags-activity-chart/tags-activity-chart.component';
import { AdminTagsListComponent } from './admin-tags-list/admin-tags-list.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AdminTagsTableComponent } from './admin-tags-table/admin-tags-table.component';

@NgModule({
  declarations: [TagsActivityChartComponent, AdminTagsTableComponent, AdminTagsListComponent],
  imports: [
    CommonModule,
    AdminTagsRoutingModule,
    NgxChartsModule,
    TagsModule
  ],
  exports: [TagsActivityChartComponent, AdminTagsTableComponent, AdminTagsListComponent]
})
export class AdminTagsModule { }
