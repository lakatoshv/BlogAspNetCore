import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { UsersService } from './core/services/users-services/users-service.service';
import { AuthGuard } from './core/guards/AuthGuard';
import { CustomToastrService } from './core/services/custom-toastr.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ErrorInterceptorService } from './core/services/global-service/error-interceptor-service';
import { GlobalService } from './core/services/global-service/global-service.service';
import { HttpClientService } from './core/services/global-service/http-client-services/http-client.service';
import { HttpInterceptorService } from './core/services/global-service/http-client-services/http-interceptor-service';

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(
      BrowserAnimationsModule,
      ToastrModule.forRoot(),
      HttpClientModule,
    ),
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes), provideClientHydration(withEventReplay()),
    UsersService,
    AuthGuard,
    CustomToastrService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptorService, multi: true },
    CustomToastrService,
    GlobalService,
    HttpClientService,
  ]
};
