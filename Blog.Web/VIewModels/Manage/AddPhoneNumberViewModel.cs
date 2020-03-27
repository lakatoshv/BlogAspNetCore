namespace BLog.Web.ViewModels.Manage
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Add phone number view model
    /// </summary>
    public class AddPhoneNumberViewModel
    {
        /// <summary>
        /// Gets or sets phoneNumber.
        /// </summary>
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
