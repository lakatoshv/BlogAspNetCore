import { FormGroup, FormControl } from '@angular/forms';

/**
 * Post add/edit form.
 */
export class PostForm {
    public postForm = new FormGroup({
        /**
         * Title input field.
         */
        title: new FormControl(''),

        /**
         * Description input field.
         */
        description: new FormControl(''),

        /**
         * Content input field.
         */
        content: new FormControl(''),

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
