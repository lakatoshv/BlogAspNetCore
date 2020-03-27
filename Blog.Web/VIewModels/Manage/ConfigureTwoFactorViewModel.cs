namespace BLog.Web.ViewModels.Manage
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// Configure two factor view model.
    /// </summary>
    public class ConfigureTwoFactorViewModel
    {
        /// <summary>
        /// Gets or sets selectedProvider.
        /// </summary>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Gets or sets providers.
        /// </summary>
        public ICollection<SelectListItem> Providers { get; set; }
    }
}
