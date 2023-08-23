import { ChartItem } from './ChartItem';
/**
 * Chart Data model.
 */
export class ChartDataModel {
    /**
     * @param name string
     * @param series ChartItem[]
     */
    constructor (
        public name: string,
        public series: ChartItem[]
    ) {}
}
