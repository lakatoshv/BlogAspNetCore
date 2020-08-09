namespace Blog.Web.VIewModels.Posts
{
    using System.Collections.Generic;
    using Core.Helpers;

    /// <summary>
    /// Posts view model.
    /// </summary>
    public class PostsViewModel
    {
        /// <summary>
        /// Gets or sets posts.
        /// </summary>
        public IList<PostViewModel> Posts { get; set; }

        /// <summary>
        /// Gets or sets display type.
        /// </summary>
        public string DisplayType { get; set; }

        /// <summary>
        /// Gets or sets page info.
        /// </summary>
        public PageInfo PageInfo { get; set; }
    }
}
