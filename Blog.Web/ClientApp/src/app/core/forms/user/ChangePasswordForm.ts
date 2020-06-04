import { FormGroup, FormControl } from '@angular/forms';

export class ChangePasswordForm {
  public profileForm = new FormGroup({

      /**
       * Old Password input field.
       */
      oldPassword: new FormControl(''),

      /**
       * New Password input field.
       */
      newPassword: new FormControl(''),

      /**
       * Confirm password field.
       */
      confirmPassword: new FormControl(''),
  });
}
