using Blog.Web.ViewModels.AspNetUser;
using System.Collections.Generic;

namespace BLog.Web.ViewModels.AspNetUser
{
    /// <summary>
    /// User table view model.
    /// </summary>
    public class UserTableViewModel
    {
        /// <summary>
        /// Gets or sets users.
        /// </summary>
        public IList<UserItemViewModel> Users { get; set; }

        /// <summary>
        /// Gets or sets recordsFiltered.
        /// </summary>
        public int RecordsFiltered { get; set; }
    }
}
