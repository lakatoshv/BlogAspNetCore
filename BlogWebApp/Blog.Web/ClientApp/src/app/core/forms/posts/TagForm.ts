import { FormGroup, FormControl, Validators } from '@angular/forms';

/**
 * Tag add/edit form.
 */
export class TagForm {
    public tagForm = new FormGroup({
        /**
         * title input field.
         */
        title: new FormControl('', [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(255)
        ]),
    });
}
