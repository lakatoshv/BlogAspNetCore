/**
 * TinyMCE Options Object model.
 */
export class TinyMCEOptionsObject {
    /**
     * TinyMCE plugins
     * @param plugins string
     * TinyMCE menubar
     * @param menubar string
     * TinyMCE toolbar
     * @param toolbar string
     */
    constructor(
        public plugins: string,
        public menubar: string,
        public toolbar: string
    ) {}
}
