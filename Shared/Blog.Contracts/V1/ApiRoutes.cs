namespace Blog.Contracts.V1
{
    /// <summary>
    /// Api routes constants.
    /// </summary>
    public static class ApiRoutes
    {
        /// <summary>
        /// The root.
        /// </summary>
        public const string Root = "api";

        /// <summary>
        /// The version.
        /// </summary>
        public const string Version = "v1";

        /// <summary>
        /// The base.
        /// </summary>
        public const string Base = Root + "/" + Version;

        /// <summary>
        /// Accounts controller routes constants.
        /// </summary>
        public static class AccountsController
        {
            /// <summary>
            /// The accounts.
            /// </summary>
            public const string Accounts = Base + "/accounts";

            /// <summary>
            /// The initialize.
            /// </summary>
            public const string Initialize = "initialize/{userId}";

            /// <summary>
            /// The get all users.
            /// </summary>
            public const string GetAllUsers = "get-all-users";

            /// <summary>
            /// The send confirmation email.
            /// </summary>
            public const string SendConfirmationEmail = "send-confirmation-email";

            /// <summary>
            /// The login.
            /// </summary>
            public const string Login = "login";

            /// <summary>
            /// The register.
            /// </summary>
            public const string Register ="register";

            /// <summary>
            /// The change password.
            /// </summary>
            public const string ChangePassword = "change-password";
        }

        /// <summary>
        /// Comments controller routes constants.
        /// </summary>
        public static class CommentsController
        {
            /// <summary>
            /// The comments.
            /// </summary>
            public const string Comments = Base + "/comments";

            /// <summary>
            /// The get comments by post.
            /// </summary>
            public const string GetCommentsByPost = "get-comments-by-post/{id}";

            /// <summary>
            /// The get comments by filter.
            /// </summary>
            public const string GetCommentsByFilter = "get-comments-by-filter";

            /// <summary>
            /// The get comment.
            /// </summary>
            public const string GetComment = "get-comment";

            /// <summary>
            /// The create comment.
            /// </summary>
            public const string CreateComment = "create";
        }

        /// <summary>
        /// Messages controller routes constants.
        /// </summary>
        public static class MessagesController
        {
            /// <summary>
            /// The messages.
            /// </summary>
            public const string Messages = Base + "/messages";

            /// <summary>
            /// The get recipient messages.
            /// </summary>
            public const string GetRecipientMessages = "get-recipient-messages/{recipientId}";

            /// <summary>
            /// The get sender messages.
            /// </summary>
            public const string GetSenderMessages = "get-sender-messages/{senderEmail}";

            /// <summary>
            /// The show.
            /// </summary>
            public const string Show = "show/{id}";
        }

        /// <summary>
        /// Posts controller routes constants.
        /// </summary>
        public static class PostsController
        {
            /// <summary>
            /// The posts.
            /// </summary>
            public const string Posts = Base + "/posts";

            /// <summary>
            /// The get posts.
            /// </summary>
            public const string GetPosts = "get-posts";

            /// <summary>
            /// The user posts.
            /// </summary>
            public const string UserPosts = "user-posts/{id}";

            /// <summary>
            /// The show.
            /// </summary>
            public const string Show = "show/{id}";

            /// <summary>
            /// The like post.
            /// </summary>
            public const string LikePost = "like/{id}";

            /// <summary>
            /// The dislike post.
            /// </summary>
            public const string DislikePost = "dislike/{id}";
        }

        /// <summary>
        /// Profile controller routes constants.
        /// </summary>
        public static class ProfileController
        {
            /// <summary>
            /// The profile.
            /// </summary>
            public const string Profile = Base + "/profile";
        }

        /// <summary>
        /// Tags controller routes constants.
        /// </summary>
        public static class TagsController
        {
            /// <summary>
            /// The tags.
            /// </summary>
            public const string Tags = Base + "/tags";

            /// <summary>
            /// The get tags.
            /// </summary>
            public const string GetTags = "get-tags";

            /// <summary>
            /// The get tags by filter.
            /// </summary>
            public const string GetTagsByFilter = "get-tags-by-filter";

            /// <summary>
            /// The get available tags.
            /// </summary>
            public const string GetAvailableTags = "get-available-tags/{postId}";

            /// <summary>
            /// The get tag.
            /// </summary>
            public const string GetTag = "get-tag";

            /// <summary>
            /// The create tag.
            /// </summary>
            public const string CreateTag = "create";
        }
    }
}