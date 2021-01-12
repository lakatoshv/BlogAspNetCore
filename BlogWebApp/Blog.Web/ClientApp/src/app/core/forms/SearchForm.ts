import { FormGroup, FormControl } from '@angular/forms';

/**
 * Search form.
 */
export class SearchForm {
    public searchForm = new FormGroup({
        /**
         * Search input field.
         */
        search: new FormControl(''),
    });
}