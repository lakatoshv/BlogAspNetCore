import { FormGroup, FormControl } from '@angular/forms';

/**
 * Profile user add/edit form.
 */
export class ProfileForm {
    public profileForm = new FormGroup({
        /**
         * User Name input field.
         */
        userName: new FormControl(''),

        /**
         * Email input field.
         */
        email: new FormControl(''),

        /**
         * First Name input field.
         */
        firstName: new FormControl(''),

        /**
         * Last Name input field.
         */
        lastName: new FormControl(''),

        /**
         * Phone Number input field.
         */
        phoneNumber: new FormControl(''),

        /**
         * Old Password input field.
         */
        oldPassword: new FormControl(''),

        /**
         * New Password input field.
         */
        newPassword: new FormControl(''),

        /**
         * About input field.
         */
        about: new FormControl(''),
    });
}
