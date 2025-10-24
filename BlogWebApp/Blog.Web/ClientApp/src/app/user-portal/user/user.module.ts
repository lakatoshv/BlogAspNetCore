import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { AuthorizationComponent } from './authorization/authorization.component';
import { ReactiveFormsModule } from '@angular/forms';
import { EditorModule } from '@tinymce/tinymce-angular';
import { RegistrationComponent } from './registration/registration.component';
import { HttpClientService } from '../../core/services/global-service/http-client-services/http-client.service';
import { AccountsService } from '../../core/services/users-services/account.sevice';
import { UsersService } from '../../core/services/users-services/users-service.service';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  imports: [
    CommonModule,
    UserRoutingModule,
    ReactiveFormsModule,
    EditorModule,
    HttpClientModule,
  ],
  declarations: [
    AuthorizationComponent,
    RegistrationComponent
  ],
  providers: [UsersService, HttpClientService, AccountsService],
  exports: [
    AuthorizationComponent,
    RegistrationComponent
  ]
})
export class UserModule { }
