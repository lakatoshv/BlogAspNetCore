namespace BLog.Web.ViewModels.Settings;

using System.Collections.Generic;
using Blog.Web.ViewModels.Settings;

/// <summary>
/// Settings list view model.
/// </summary>
public class SettingsListViewModel
{
    /// <summary>
    /// Gets or sets settings.
    /// </summary>
    public IEnumerable<SettingViewModel> Settings { get; set; }
}