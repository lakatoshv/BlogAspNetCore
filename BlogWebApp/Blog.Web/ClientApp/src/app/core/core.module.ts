import { MessagesService } from 'src/app/core/services/messages-service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GeneralServiceService } from './services/general-service.service';
import { HttpClientService } from './services/global-service/http-client-services/http-client.service';
import { PostService } from './services/posts-services/post.service';
import { UsersService } from './services/users-services/users.service';
import { AccountsService } from './services/users-services/account.sevice';
import { HttpInterceptorService } from './services/global-service/http-client-services/HttpInterceptorService ';
import { ErrorInterceptorService } from './services/global-service/ErrorInterceptorService';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers: [
    GeneralServiceService,
    HttpClientService,
    PostService,
    UsersService,
    AccountsService,
    MessagesService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptorService, multi: true }
  ],
})
export class CoreModule { }
