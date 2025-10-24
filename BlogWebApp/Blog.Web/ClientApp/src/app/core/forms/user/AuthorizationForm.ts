import { FormGroup, FormControl, Validators } from '@angular/forms';

/**
 * Authorization user form.
 */
export class AuthorizationForm {
    public authorizationForm = new FormGroup({
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
        password: new FormControl(null, [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(255),
        ])
    });
}
