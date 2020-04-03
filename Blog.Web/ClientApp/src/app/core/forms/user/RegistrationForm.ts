import { FormGroup, FormControl } from '@angular/forms';

/**
 * Registration user form.
 */
export class RegistrationForm {
  public registrationForm = new FormGroup({
    /**
     * First Name input field.
     */
    firstName: new FormControl(''),

    /**
     * Last Name input field.
     */
    lastName: new FormControl(''),

    /**
     * Email input field.
     */
    email: new FormControl(''),

    /**
     * Password input field.
     */
    password: new FormControl(''),

    /**
     * Confirm Password input field.
     */
    confirmPassword: new FormControl(''),

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
