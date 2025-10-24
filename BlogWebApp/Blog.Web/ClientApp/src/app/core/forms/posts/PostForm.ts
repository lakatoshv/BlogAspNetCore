import { FormGroup, FormControl, Validators } from '@angular/forms';

/**
 * Post add/edit form.
 */
export class PostForm {
    public postForm = new FormGroup({
        /**
         * Title input field.
         */
        title: new FormControl('', [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(255)
        ]),

        /**
         * Description input field.
         */
        description: new FormControl('', [
            Validators.required,
            Validators.minLength(3),
        ]),

        /**
         * Content input field.
         */
        content: new FormControl('', [
            Validators.required,
            Validators.minLength(3),
        ]),

        /**
         * imageUrl input field.
         */
        imageUrl: new FormControl(''),

        /**
         * Tags input field.
         */
        tags: new FormControl(''),

        /**
         * Date input field.
         */
        date: new FormControl(''),
    });
}
