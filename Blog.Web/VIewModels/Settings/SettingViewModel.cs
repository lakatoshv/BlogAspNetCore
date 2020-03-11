using Blog.Core.Mapping.Interfaces;
using Blog.Data.Models;

namespace Blog.Web.ViewModels.Settings
{

    public class SettingViewModel : IMapFrom<Setting>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
