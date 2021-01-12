using Blog.Core.Mapping.Interfaces;
using Blog.Data.Models;

namespace Blog.Web.ViewModels.Settings
{
    /// <summary>
    /// Setting view model.
    /// </summary>
    public class SettingViewModel : IMapFrom<Setting>
    {
        /// <summary>
        /// Gets or sets id.l
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public string Value { get; set; }
    }
}
