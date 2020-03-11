using Blog.Web.ViewModels.AspNetUser;
using System.Collections.Generic;

namespace BLog.Web.ViewModels.AspNetUser
{
    public class UserTableViewModel
    {
        public IList<UserItemViewModel> Users { get; set; }

        public int RecordsFiltered { get; set; }
    }
}
