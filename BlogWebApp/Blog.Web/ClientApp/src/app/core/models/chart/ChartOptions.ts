import { Color } from '@swimlane/ngx-charts';
import { ChartDataModel } from './ChartDataModel';

/**
 * Chart Options model.
 */
export class ChartOptions {
    /**
     * @param Data ChartDataModel[]
     * @param View any[]
     * @param ShowXAxis boolean
     * @param ShowYAxis boolean
     * @param Gradient boolean
     * @param ShowLegend boolean
     * @param ShowXAxisLabel boolean
     * @param ShowYAxisLabel boolean
     * @param Timeline boolean
     * @param AutoScale boolean
     * @param ColorScheme object
     */
    constructor(
        public Data: ChartDataModel[],
        public View: [number, number],
        public ShowXAxis: boolean,
        public ShowYAxis: boolean,
        public Gradient: boolean,
        public ShowLegend: boolean,
        public ShowXAxisLabel: boolean,
        public ShowYAxisLabel: boolean,
        public Timeline: boolean,
        public AutoScale: boolean,
        public ColorScheme: Color,
    ) {}
}
