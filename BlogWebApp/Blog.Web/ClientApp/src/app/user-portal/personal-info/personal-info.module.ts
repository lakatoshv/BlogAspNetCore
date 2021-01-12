import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ChangeEmailComponent } from './change-email/change-email.component';
import { ChangePhoneNumberComponent } from './change-phone-number/change-phone-number.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [ChangePasswordComponent, ChangeEmailComponent, ChangePhoneNumberComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [ChangePasswordComponent, ChangeEmailComponent, ChangePhoneNumberComponent]
})
export class PersonalInfoModule { }
