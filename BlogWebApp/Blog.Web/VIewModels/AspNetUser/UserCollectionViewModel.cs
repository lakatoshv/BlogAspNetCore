namespace BLog.Web.ViewModels.AspNetUser;

using System.Collections.Generic;
using Blog.Web.ViewModels.AspNetUser;

/// <summary>
/// User collection view model
/// </summary>
public class UserCollectionViewModel
{
    /// <summary>
    /// Gets or sets users.
    /// </summary>
    public IList<UserItemViewModel> Users { get; set; }
}