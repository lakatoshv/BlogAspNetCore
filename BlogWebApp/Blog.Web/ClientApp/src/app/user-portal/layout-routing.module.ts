import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponentComponent } from './layout-component/layout-component.component';
import { AuthorizationComponent } from './user/authorization/authorization.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { ProfilePageComponent } from './profile/profile-page/profile-page.component';
import { EditProfileComponent } from './profile/edit-profile/edit-profile.component';
import { ChangeEmailComponent } from './personal-info/change-email/change-email.component';
import { ChangePasswordComponent } from './personal-info/change-password/change-password.component';
import { ChangePhoneNumberComponent } from './personal-info/change-phone-number/change-phone-number.component';
import { NotFoundComponent } from '../shared/errors/not-found/not-found.component';
import { AuthGuard } from '../core/guards/AuthGuard';
import { ContactsComponent } from './default-pages/contacts/contacts.component';
import { AboutComponent } from './default-pages/about/about.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponentComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('../shared/posts/posts.module').then(m => m.PostsModule)
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
        canActivate: [AuthGuard],
        loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule)
      },
      {
        path: 'my-profile',
        canActivate: [AuthGuard],
        component: ProfilePageComponent
      },
      {
        path: 'my-profile/edit',
        canActivate: [AuthGuard],
        component: EditProfileComponent
      },
      {
        path: 'my-profile/personal-info/change-email',
        canActivate: [AuthGuard],
        component: ChangeEmailComponent
      },
      {
        path: 'my-profile/personal-info/change-password',
        canActivate: [AuthGuard],
        component: ChangePasswordComponent
      },
      {
        path: 'my-profile/personal-info/change-phone-number',
        canActivate: [AuthGuard],
        component: ChangePhoneNumberComponent
      },
      {
        path: 'not-found',
        component: NotFoundComponent
      },
      {
        path: '**',
        component: NotFoundComponent
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
