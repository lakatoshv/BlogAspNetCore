namespace BLog.Web.ViewModels.Manage
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Manage logins view model.
    /// </summary>
    public class ManageLoginsViewModel
    {
        /// <summary>
        /// Gets or sets currentLogins.
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        /// Gets or sets otherLogins.
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }
    }
}
