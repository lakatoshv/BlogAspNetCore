import { FormGroup, FormControl } from '@angular/forms';

/**
 * Message form.
 */
export class MessageForm {
    public messageForm = new FormGroup({
        /**
         * Sender Name input field.
         */
        senderName: new FormControl(''),

        /**
         * Sender Email input field.
         */
        senderEmail: new FormControl(''),

        /**
         * Subject input field.
         */
        subject: new FormControl('')
    });
}
