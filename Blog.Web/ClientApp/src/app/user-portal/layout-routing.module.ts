import { ChangePhoneNumberComponent } from './personal-info/change-phone-number/change-phone-number.component';
import { ChangePasswordComponent } from './personal-info/change-password/change-password.component';
import { ChangeEmailComponent } from './personal-info/change-email/change-email.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponentComponent } from './layout-component/layout-component.component';
import { AboutComponent } from './default-pages/about/about.component';
import { ContactsComponent } from './default-pages/contacts/contacts.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { AuthorizationComponent } from './user/authorization/authorization.component';
import { ProfilePageComponent } from './profile/profile-page/profile-page.component';
import { EditProfileComponent } from './profile/edit-profile/edit-profile.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponentComponent,
    children: [
      {
        path: '',
        loadChildren: '../shared/posts/posts.module#PostsModule'
      },
      {
        path: 'about',
        component: AboutComponent
      },
      {
        path: 'contacts',
        component: ContactsComponent
      },
      {
        path: 'registration',
        component: RegistrationComponent
      },
      {
        path: 'authorization',
        component: AuthorizationComponent
      },
      {
        path: 'profile',
        loadChildren: '../user-portal/profile/profile.module#ProfileModule'
      },
      {
        path: 'my-profile',
        component: ProfilePageComponent
      },
      {
        path: 'my-profile/edit',
        component: EditProfileComponent
      },
      {
        path: 'my-profile/personal-info/change-email',
        component: ChangeEmailComponent
      },
      {
        path: 'my-profile/personal-info/change-password',
        component: ChangePasswordComponent
      },
      {
        path: 'my-profile/personal-info/change-phone-number',
        component: ChangePhoneNumberComponent
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
