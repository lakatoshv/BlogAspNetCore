import { PersonalInfoModule } from './personal-info/personal-info.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponentComponent } from './layout-component/layout-component.component';
import { ReactiveFormsModule } from '@angular/forms';
import { EditorModule } from '@tinymce/tinymce-angular';
import { ProfileModule } from './profile/profile.module';
import { ErrorsModule } from '../shared/errors/errors.module';
import { DefaultPagesModule } from './default-pages/default-pages.module';
import { UserModule } from './user/user.module';
// import { SharedModule } from "@app/shared";

@NgModule({
  imports: [
    CommonModule,
    LayoutRoutingModule,
    ReactiveFormsModule,
    EditorModule,
    ProfileModule,
    PersonalInfoModule,
    ErrorsModule,
    DefaultPagesModule,
    UserModule
  ],
  declarations: [
    LayoutComponentComponent
  ],
})
export class LayoutModule { }
