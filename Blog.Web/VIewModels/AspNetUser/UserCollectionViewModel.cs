using Blog.Web.ViewModels.AspNetUser;
using System.Collections.Generic;

namespace BLog.Web.ViewModels.AspNetUser
{
    public class UserCollectionViewModel
    {
        public IList<UserItemViewModel> Users { get; set; }
    }
}