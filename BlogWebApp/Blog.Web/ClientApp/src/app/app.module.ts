import { ErrorsModule } from './shared/errors/errors.module';
import { CustomToastrService } from './core/services/custom-toastr.service';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpInterceptorService } from './core/services/global-service/http-client-services/HttpInterceptorService ';
import { ErrorInterceptorService } from './core/services/global-service/ErrorInterceptorService';
import { ToastrModule } from 'ngx-toastr';
import { GlobalService } from './core/services/global-service/global-service.service';
import { AuthGuard } from './core/guards/AuthGuard';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { UsersService } from './core/services/users-services/users.service';
import { HttpClientService } from './core/services/global-service/http-client-services/http-client.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    ToastrModule.forRoot(),
    ErrorsModule,
    BsDropdownModule.forRoot()
  ],
  bootstrap: [AppComponent],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptorService, multi: true },
    CustomToastrService,
    GlobalService,
    HttpClientService,
    UsersService,
    AuthGuard
  ]
})
export class AppModule { }

