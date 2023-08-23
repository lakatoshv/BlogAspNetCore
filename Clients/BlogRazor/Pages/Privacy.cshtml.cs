namespace BlogRazor.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

/// <summary>
/// Privacy model.
/// </summary>
/// <seealso cref="PageModel" />
public class PrivacyModel : PageModel
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<PrivacyModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrivacyModel"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Called when [get].
    /// </summary>
    public void OnGet()
    {
    }
}