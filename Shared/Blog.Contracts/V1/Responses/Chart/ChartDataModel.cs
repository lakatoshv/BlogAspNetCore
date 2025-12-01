using System.Collections.Generic;

namespace Blog.Contracts.V1.Responses.Chart;

/// <summary>
/// The Chart Data model.
/// </summary>
public class ChartDataModel
{
    /// <summary>
    /// Gets or sets name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets series.
    /// </summary>
    public List<ChartItem> Series { get; set; } = [];
}