namespace BlogRazor.Pages;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Blog.Contracts.V1.Requests.PostsRequests;
using Blog.Contracts.V1.Responses.PostsResponses;
using Services.Interfaces;

/// <summary>
/// Index model.
/// </summary>
/// <seealso cref="PageModel" />
/// <remarks>
/// Initializes a new instance of the <see cref="IndexModel"/> class.
/// </remarks>
/// <param name="logger">The logger.</param>
/// <param name="postsService">The posts service.</param>
public class IndexModel(
    ILogger<IndexModel> logger,
    IPostsService postsService)
    : PageModel
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<IndexModel> _logger = logger;

    /// <summary>
    /// Gets or sets the posts.
    /// </summary>
    /// <value>
    /// The posts.
    /// </value>
    [BindProperty]
    public PagedPostsResponse PagedPosts { get; set; } = new ();

    /// <summary>
    /// Gets or sets a value indicating whether this instance is loaded.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
    /// </value>
    [BindProperty]
    public bool IsLoaded { get; set; }

    private readonly IPostsService _postsService = postsService;

    /// <summary>
    /// Called when [get].
    /// </summary>
    public async Task OnGet()
    {
        try
        {
            var pagedProductsResponse = await _postsService.GetPosts(new PostsSearchParametersRequest());

            if (pagedProductsResponse != null && pagedProductsResponse.Posts.Count > 0)
            {
                PagedPosts = pagedProductsResponse;
                IsLoaded = true;
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }
}