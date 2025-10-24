import { FormGroup, FormControl, Validators } from '@angular/forms';

/**
 * Comment add/edit form.
 */
export class CommentForm {
    public commentForm = new FormGroup({
        /**
         * Content input field.
         */
        content: new FormControl('', [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(255)
        ]),

        /**
         * Email input field.
         */
        email: new FormControl(''),

        /**
         * Name input field.
         */
        name: new FormControl(''),
    });
}
