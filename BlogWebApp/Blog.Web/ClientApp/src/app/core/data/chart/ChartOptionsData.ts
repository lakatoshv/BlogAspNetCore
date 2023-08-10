import { ChartData } from './ChartData';
import { ChartOptions } from '../../models/chart/ChartOptions';
import { Color, ScaleType } from '@swimlane/ngx-charts';

/**
 * Chart options default data.
 */
export const ChartOptionsData: ChartOptions = {
    Data: ChartData,
    View: [1600, 300],
    ShowXAxis: true,
    ShowYAxis: true,
    Gradient: false,
    ShowLegend: true,
    ShowXAxisLabel: true,
    ShowYAxisLabel: true,
    Timeline: true,
    ColorScheme: {
        group: ScaleType.Ordinal, 
        selectable: true, 
        name: 'Customer Usage',
        domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
    },
    AutoScale: true
};
