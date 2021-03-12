using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BlogRazor.Pages
{
    /// <summary>
    /// Index model.
    /// </summary>
    /// <seealso cref="PageModel" />
    public class IndexModel : PageModel
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public IndexModel(ILogger<IndexModel> logger)
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
}
