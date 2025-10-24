import { FormGroup, FormControl, Validators } from '@angular/forms';

/**
 * Registration user form.
 */
export class RegistrationForm {
    public registrationForm = new FormGroup({
        /**
         * First Name input field.
         */
        firstName: new FormControl('', [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(255),
        ]),

        /**
         * Last Name input field.
         */
        lastName: new FormControl('', [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(255),
        ]),

        /**
         * Email input field.
         */
        email: new FormControl('', [
            Validators.email,
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(255),
        ]),

        /**
         * Password input field.
         */
        password: new FormControl('', [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(255),
        ]),

        /**
         * Confirm Password input field.
         */
        confirmPassword: new FormControl('', [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(255),
        ]),

        /**
         * Phone Number input field.
         */
        phoneNumber: new FormControl(''),

        /**
         * Roles input field.
         */
        roles: new FormControl(''),
    });
}
