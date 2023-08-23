using System.Collections.Generic;

namespace Blog.Contracts.V1.Responses.Chart;

public class ChartDataModel
{
    public string Name { get; set; }
    public List<ChartItem> Series { get; set; } = new();
}