namespace Blog.FSharp.Contracts.V1

/// <summary>
/// API routes constants.
/// </summary>
module ApiRoutes =

    /// <summary>The root.</summary>
    let Root = "api"

    /// <summary>The version.</summary>
    let Version = "v1"

    /// <summary>The base route.</summary>
    let Base = Root + "/" + Version

    /// <summary>Accounts controller routes constants.</summary>
    module AccountsController =
        /// <summary>The accounts.</summary>
        let Accounts = Base + "/accounts"
        /// <summary>The initialize route.</summary>
        let Initialize = "initialize/{userId}"
        /// <summary>Get all users.</summary>
        let GetAllUsers = "get-all-users"
        /// <summary>Send confirmation email.</summary>
        let SendConfirmationEmail = "send-confirmation-email"
        /// <summary>Login route.</summary>
        let Login = "login"
        /// <summary>Register route.</summary>
        let Register = "register"
        /// <summary>Change password route.</summary>
        let ChangePassword = "change-password"
        /// <summary>Users activity route.</summary>
        let UsersActivity = "users-activity"

    /// <summary>Comments controller routes constants.</summary>
    module CommentsController =
        /// <summary>Comments route.</summary>
        let Comments = Base + "/comments"
        /// <summary>Get comments by post route.</summary>
        let GetCommentsByPost = "get-comments-by-post/{id:int}"
        /// <summary>Get comments by filter route.</summary>
        let GetCommentsByFilter = "get-comments-by-filter"
        /// <summary>Get comment route.</summary>
        let GetComment = "get-comment"
        /// <summary>Create comment route.</summary>
        let CreateComment = "create"
        /// <summary>Edit comment route.</summary>
        let EditComment = "{id:int}"
        /// <summary>Delete comment route.</summary>
        let DeleteComment = "{id:int}"
        /// <summary>Comments activity route.</summary>
        let CommentsActivity = "comments-activity"

    /// <summary>Messages controller routes constants.</summary>
    module MessagesController =
        /// <summary>Messages route.</summary>
        let Messages = Base + "/messages"
        /// <summary>Get recipient messages route.</summary>
        let GetRecipientMessages = "get-recipient-messages/{recipientId}"
        /// <summary>Get sender messages route.</summary>
        let GetSenderMessages = "get-sender-messages/{senderEmail}"
        /// <summary>Show route.</summary>
        let Show = "show/{id:int}"

    /// <summary>Posts controller routes constants.</summary>
    module PostsController =
        /// <summary>Posts route.</summary>
        let Posts = Base + "/posts"
        /// <summary>Get posts route.</summary>
        let GetPosts = "get-posts"
        /// <summary>User posts route.</summary>
        let UserPosts = "user-posts/{id}"
        /// <summary>Show route.</summary>
        let Show = "show/{id:int}"
        /// <summary>Like post route.</summary>
        let LikePost = "like/{id:int}"
        /// <summary>Dislike post route.</summary>
        let DislikePost = "dislike/{id:int}"
        /// <summary>Posts activity route.</summary>
        let PostsActivity = "posts-activity"

    /// <summary>Profile controller routes constants.</summary>
    module ProfileController =
        /// <summary>Profile route.</summary>
        let Profile = Base + "/profile"

    /// <summary>Tags controller routes constants.</summary>
    module TagsController =
        /// <summary>Tags route.</summary>
        let Tags = Base + "/tags"
        /// <summary>Get tags.</summary>
        let GetTags = "get-tags"
        /// <summary>Get tags by filter route.</summary>
        let GetTagsByFilter = "get-tags-by-filter"
        /// <summary>Get available tags route.</summary>
        let GetAvailableTags = "get-available-tags/{postId:int}"
        /// <summary>Get tag route.</summary>
        let GetTag = "get-tag/{id:int}"
        /// <summary>Create tag route.</summary>
        let CreateTag = "create"
        /// <summary>Tags activity route.</summary>
        let TagsActivity = "tags-activity"

    /// <summary>Health controller routes constants.</summary>
    module HealthController =
        /// <summary>Health route.</summary>
        let Health = Base + "/health-check"

    /// <summary>Exports controller routes constants.</summary>
    module ExportsController =
        /// <summary>Exports route.</summary>
        let Exports = Base + "/exports"
