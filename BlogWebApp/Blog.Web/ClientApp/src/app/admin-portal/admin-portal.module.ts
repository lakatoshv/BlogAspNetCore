import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminPortalRoutingModule } from './admin-portal-routing.module';
import { LayoutComponentComponent } from './layout-component/layout-component.component';
import { DefaultPagesModule } from './default-pages/default-pages.module';
import { ErrorsModule } from '../shared/errors/errors.module';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    LayoutComponentComponent
  ],
  imports: [
    CommonModule,
    AdminPortalRoutingModule,
    DefaultPagesModule,
    ErrorsModule,
    ToastrModule.forRoot()
  ]
})
export class AdminPortalModule { }
