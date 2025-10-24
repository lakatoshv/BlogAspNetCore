import { PostsService } from './services/posts-services/posts.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GeneralServiceService } from './services/general-service.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptorService } from './services/global-service/error-interceptor-service';
import { HttpClientService } from './services/global-service/http-client-services/http-client.service';
import { HttpInterceptorService } from './services/global-service/http-client-services/http-interceptor-service';
import { MessagesService } from './services/messages-service';
import { AccountsService } from './services/users-services/account.sevice';
import { UsersService } from './services/users-services/users-service.service';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers: [
    GeneralServiceService,
    HttpClientService,
    PostsService,
    UsersService,
    AccountsService,
    MessagesService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptorService, multi: true }
  ],
})
export class CoreModule { }
