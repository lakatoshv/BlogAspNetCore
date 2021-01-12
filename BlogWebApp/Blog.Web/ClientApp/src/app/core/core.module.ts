import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GeneralServiceService } from './services/general-service.service';
import { HttpClientService } from './services/global-service/http-client-services/http-client.service';
import { PostService } from './services/posts-services/post.service';
import { UsersService } from './services/users-services/users.service';
import { AccountsService } from './services/users-services/account.sevice';
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
    AccountsService
  ]
})
export class CoreModule { }
