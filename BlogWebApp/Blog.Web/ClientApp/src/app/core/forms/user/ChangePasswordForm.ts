import { FormGroup, FormControl, Validators } from '@angular/forms';

export class ChangePasswordForm {
  public profileForm = new FormGroup({

      /**
       * Old Password input field.
       */
      oldPassword: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(255),
      ]),

      /**
       * New Password input field.
       */
      newPassword: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(255),
      ]),

      /**
       * Confirm password field.
       */
      confirmPassword: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(255),
      ]),
  });
}
