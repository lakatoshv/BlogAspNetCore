import { ErrorsModule } from './../shared/errors/errors.module';
import { UserModule } from './user/user.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponentComponent } from './layout-component/layout-component.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { ReactiveFormsModule } from '@angular/forms';
import { EditorModule } from '@tinymce/tinymce-angular';
import { AboutComponent } from './default-pages/about/about.component';
import { ContactsComponent } from './default-pages/contacts/contacts.component';
import { ProfileModule } from './profile/profile.module';
import { PersonalInfoModule } from './personal-info/personal-info.module';
// import { SharedModule } from "@app/shared";

@NgModule({
  imports: [
    CommonModule,
    LayoutRoutingModule,
    AngularFontAwesomeModule,
    ReactiveFormsModule,
    EditorModule,
    UserModule,
    ProfileModule,
    PersonalInfoModule,
    ErrorsModule
  ],
  declarations: [
    LayoutComponentComponent,
    AboutComponent,
    ContactsComponent,
  ],
})
export class LayoutModule { }
