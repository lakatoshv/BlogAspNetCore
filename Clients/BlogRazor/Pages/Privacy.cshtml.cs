namespace BlogRazor.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

/// <summary>
/// Privacy model.
/// </summary>
/// <seealso cref="PageModel" />
/// <remarks>
/// Initializes a new instance of the <see cref="PrivacyModel"/> class.
/// </remarks>
/// <param name="logger">The logger.</param>
public class PrivacyModel(ILogger<PrivacyModel> logger)
    : PageModel
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<PrivacyModel> _logger = logger;

    /// <summary>
    /// Called when [get].
    /// </summary>
    public void OnGet()
    {
    }
}