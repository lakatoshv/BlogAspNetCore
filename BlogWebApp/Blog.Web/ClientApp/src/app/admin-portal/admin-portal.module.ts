import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminPortalRoutingModule } from './admin-portal-routing.module';
import { LayoutComponentComponent } from './layout-component/layout-component.component';
import { DefaultPagesModule } from './default-pages/default-pages.module';
import { ErrorsModule } from '../shared/errors/errors.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpInterceptorService } from '../core/services/global-service/http-client-services/HttpInterceptorService ';
import { ErrorInterceptorService } from '../core/services/global-service/ErrorInterceptorService';
import { CustomToastrService } from '../core/services/custom-toastr.service';
import { GlobalService } from '../core/services/global-service/global-service.service';
import { AuthGuard } from '../core/guards/AuthGuard';
import { ToastrModule } from 'ngx-toastr';
import { UsersService } from '../core/services/users-services/users.service';

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
