/**
 * Page Info model.
 */
export class PageInfo {
    /**
     * @param pageSize number
     * @param pageNumber number
     * @param totalItems number
     */
    constructor(
        public pageSize: number,
        public pageNumber: number,
        public totalItems: number
    ) {}
}
