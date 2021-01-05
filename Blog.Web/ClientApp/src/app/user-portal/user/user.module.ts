import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { RegistrationComponent } from './registration/registration.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { EditorModule } from '@tinymce/tinymce-angular';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { AuthorizationComponent } from './authorization/authorization.component';
import { AccountsService } from 'src/app/core/services/users-services/account.sevice';
import { HttpClientService } from 'src/app/core/services/global-service/http-client-services/http-client.service';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';

@NgModule({
  imports: [
    CommonModule,
    UserRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    EditorModule
  ],
  declarations: [RegistrationComponent, AuthorizationComponent],
  providers: [UsersService, HttpClientService, AccountsService],
  exports: [RegistrationComponent, AuthorizationComponent]
})
export class UserModule { }
