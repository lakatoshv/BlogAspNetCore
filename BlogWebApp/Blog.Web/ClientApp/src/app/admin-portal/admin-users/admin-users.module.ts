import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminUsersRoutingModule } from './admin-users-routing.module';
import { UsersActivityChartComponent } from './users-activity-chart/users-activity-chart.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';

@NgModule({
  imports: [
    CommonModule,
    AdminUsersRoutingModule,
    NgxChartsModule,
  ],
  declarations: [UsersActivityChartComponent],
  exports: [UsersActivityChartComponent]
})
export class UsersModule { }
