import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminUsersRoutingModule } from './admin-users-routing.module';
import { UsersActivityChartComponent } from './users-activity-chart/users-activity-chart.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';


@NgModule({
  declarations: [UsersActivityChartComponent],
  imports: [
    CommonModule,
    AdminUsersRoutingModule,
    NgxChartsModule,
  ],
  exports: [UsersActivityChartComponent]
})
export class AdminUsersModule { }
