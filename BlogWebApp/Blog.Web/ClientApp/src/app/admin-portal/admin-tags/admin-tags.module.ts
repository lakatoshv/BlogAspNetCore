import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminTagsRoutingModule } from './admin-tags-routing.module';
import { TagsActivityChartComponent } from './tags-activity-chart/tags-activity-chart.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';


@NgModule({
  declarations: [
    TagsActivityChartComponent
  ],
  imports: [
    CommonModule,
    AdminTagsRoutingModule,
    NgxChartsModule,
  ],
  exports: [TagsActivityChartComponent]
})
export class AdminTagsModule { }
