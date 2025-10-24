import { FormGroup, FormControl } from '@angular/forms';

/**
 * Message form.
 */
export class MessageForm {
    public messageForm = new FormGroup({
        /**
         * Name input field.
         */
        name: new FormControl(''),

        /**
         * Email input field.
         */
        email: new FormControl(''),

        /**
         * Message input field.
         */
        message: new FormControl('')
    });
}