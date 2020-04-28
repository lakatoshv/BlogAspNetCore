import { FormGroup, FormControl } from '@angular/forms';

/**
 * Comment add/edit form.
 */
export class CommentForm {
    public commentForm = new FormGroup({
        /**
         * Content input field.
         */
        content: new FormControl(''),

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
