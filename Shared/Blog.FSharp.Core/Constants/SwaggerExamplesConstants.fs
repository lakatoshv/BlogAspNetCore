namespace Blog.FSharp.Core.Constants

/// <summary>
/// Swagger examples constants.
/// </summary>
[<AutoOpen>]
module SwaggerExamplesConstants =

    /// <summary>
    /// Create comment request example.
    /// </summary>
    module CreateCommentRequestExample =
        [<Literal>]
        let CommentBody = "Create comment request example."

    /// <summary>
    /// Update comment request example.
    /// </summary>
    module UpdateCommentRequestExample =
        [<Literal>]
        let CommentBody = "Update comment request example."

    /// <summary>
    /// Create message request example.
    /// </summary>
    module CreateMessageRequestExample =
        [<Literal>]
        let Subject = "Create message request example"

        [<Literal>]
        let Body = "Create message request example."

    /// <summary>
    /// Update message request example.
    /// </summary>
    module UpdateMessageRequestExample =
        [<Literal>]
        let Subject = "Update message request example"

        [<Literal>]
        let Body = "Update message request example."

    /// <summary>
    /// Create post request example.
    /// </summary>
    module CreatePostRequestExample =
        [<Literal>]
        let Title = "Create post request"

        [<Literal>] 
        let Description = "Create post request description"

        [<Literal>]
        let Content = "Create post request content"

        [<Literal>] 
        let ImageUrl = "ImageUrl"

    /// <summary>
    /// Update post request example.
    /// </summary>
    module UpdatePostRequestExample =
        [<Literal>] 
        let Title = "Update post request"

        [<Literal>] 
        let Description = "Update post request description"

        [<Literal>] 
        let Content = "Update post request content"

        [<Literal>] 
        let ImageUrl = "ImageUrl"

    /// <summary>
    /// Create tag request example.
    /// </summary>
    module CreateTagRequestExample =
        [<Literal>]
        let Title = "Create tag request example"

    /// <summary>
    /// Update tag request example.
    /// </summary>
    module UpdateTagRequestExample =
        [<Literal>]
        let Title = "Update tag request example"

    /// <summary>
    /// Tag request example.
    /// </summary>
    module TagRequestExample =
        [<Literal>]
        let Title = "Tag request example"

    /// <summary>
    /// Account example.
    /// </summary>
    module AccountExample =
        [<Literal>]
        let Password = "PasswordExample"

        [<Literal>]
        let Email = "email@email.email"

        [<Literal>]
        let FirstName = "FirstName"

        [<Literal>]
        let LastName = "LastName"

        [<Literal>]
        let UserName = "UserName"

        [<Literal>]
        let PhoneNumber = "0123456789"

    /// <summary>
    /// Change password request example.
    /// </summary>
    module ChangePasswordRequestExample =
        [<Literal>]
        let NewPassword = "New" + AccountExample.Password

    /// <summary>
    /// Update profile request example.
    /// </summary>
    module UpdateProfileRequestExample =
        [<Literal>]
        let Email = "updated_" + AccountExample.Email

        [<Literal>]
        let FirstName = "Updated " + AccountExample.FirstName

        [<Literal>]
        let LastName = "Updated " + AccountExample.LastName

        [<Literal>]
        let PhoneNumber = "0011223344"

        [<Literal>]
        let Password = ChangePasswordRequestExample.NewPassword

        [<Literal>]
        let About = "About"

    /// <summary>
    /// Search parameters request example.
    /// </summary>
    module SearchParametersRequestExample =
        [<Literal>]
        let Search = "Some search value"

    /// <summary>
    /// Sort parameters request example.
    /// </summary>
    module SortParametersRequestExample =
        [<Literal>]
        let OrderBy = "Title"

        [<Literal>]
        let SortBy = "Title"

        [<Literal>]
        let CurrentPage = 1

        [<Literal>]
        let PageSize = 10

    /// <summary>
    /// Comment response example.
    /// </summary>
    module CommentResponseExample =
        [<Literal>]
        let CommentBody = "Comment response."

    /// <summary>
    /// Post view response example.
    /// </summary>
    module PostViewResponseExample =
        [<Literal>]
        let Title = "Post view response"

        [<Literal>]
        let Description = "Post view response."

        [<Literal>] 
        let Content = "Post view response."

        [<Literal>]
        let ImageUrl = "ImageUrl"

    /// <summary>
    /// Tag response example.
    /// </summary>
    module TagResponseExample =
        [<Literal>]
        let Title = "Tag response"

    /// <summary>
    /// Profile response example.
    /// </summary>
    module ProfileResponseExample =
        [<Literal>]
        let About = "Profile response example"

        [<Literal>] 
        let ProfileImg = "ProfileImg"

    /// <summary>
    /// Message response example.
    /// </summary>
    module MessageResponseExample =
        [<Literal>]
        let Subject = "Message response example."

        [<Literal>]
        let Body = "Message response example."