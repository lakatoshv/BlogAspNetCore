// <copyright file="SwaggerExamplesConsts.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Consts;

/// <summary>
/// Swagger examples consts.
/// </summary>
public static class SwaggerExamplesConsts
{
    /// <summary>
    /// Create comment request example.
    /// </summary>
    public static class CreateCommentRequestExample
    {
        /// <summary>
        /// The comment body.
        /// </summary>
        public const string CommentBody = "Create comment request example.";
    }

    /// <summary>
    /// Create message request example.
    /// </summary>
    public static class UpdateCommentRequestExample
    {
        /// <summary>
        /// The comment body.
        /// </summary>
        public const string CommentBody = "Update comment request example.";
    }

    /// <summary>
    /// Create message request example.
    /// </summary>
    public static class CreateMessageRequestExample
    {
        /// <summary>
        /// The comment body.
        /// </summary>
        public const string Subject = "Create message request example";

        /// <summary>
        /// The body.
        /// </summary>
        public const string Body = "Create message request example.";
    }

    /// <summary>
    /// Update message request example.
    /// </summary>
    public static class UpdateMessageRequestExample
    {
        /// <summary>
        /// The comment body.
        /// </summary>
        public const string Subject = "Update message request example";

        /// <summary>
        /// The body.
        /// </summary>
        public const string Body = "Update message request example.";
    }

    /// <summary>
    /// Create post request example.
    /// </summary>
    public static class CreatePostRequestExample
    {
        /// <summary>
        /// The title.
        /// </summary>
        public const string Title = "Create post request";

        /// <summary>
        /// The description.
        /// </summary>
        public const string Description = "Create post request description";

        /// <summary>
        /// The content.
        /// </summary>
        public const string Content = "Create post request content";

        /// <summary>
        /// The image URL.
        /// </summary>
        public const string ImageUrl = "ImageUrl";
    }

    /// <summary>
    /// Update post request example.
    /// </summary>
    public static class UpdatePostRequestExample
    {
        /// <summary>
        /// The title.
        /// </summary>
        public const string Title = "Update post request";

        /// <summary>
        /// The description.
        /// </summary>
        public const string Description = "Update post request description";

        /// <summary>
        /// The content.
        /// </summary>
        public const string Content = "Update post request content";

        /// <summary>
        /// The image URL.
        /// </summary>
        public const string ImageUrl = "ImageUrl";
    }

    /// <summary>
    /// Create tag request example.
    /// </summary>
    public static class CreateTagRequestExample
    {
        /// <summary>
        /// The title.
        /// </summary>
        public const string Title = "Create tag request example";
    }

    /// <summary>
    /// Update tag request example.
    /// </summary>
    public static class UpdateTagRequestExample
    {
        /// <summary>
        /// The title.
        /// </summary>
        public const string Title = "Update tag request example";
    }

    /// <summary>
    /// Tag request example.
    /// </summary>
    public static class TagRequestExample
    {
        /// <summary>
        /// The title.
        /// </summary>
        public const string Title = "Tag request example";
    }

    /// <summary>
    /// Account example.
    /// </summary>
    public static class AccountExample
    {
        /// <summary>
        /// The password.
        /// </summary>
        public const string Password = "PasswordExample";

        /// <summary>
        /// The email.
        /// </summary>
        public const string Email = "email@email.email";

        /// <summary>
        /// The first name.
        /// </summary>
        public const string FirstName = "FirstName";

        /// <summary>
        /// The last name.
        /// </summary>
        public const string LastName = "LastName";

        /// <summary>
        /// The user name.
        /// </summary>
        public const string UserName = "UserName";

        /// <summary>
        /// The phone number.
        /// </summary>
        public const string PhoneNumber = "0123456789";
    }

    /// <summary>
    /// Update profile request example.
    /// </summary>
    public static class UpdateProfileRequestExample
    {
        /// <summary>
        /// The email.
        /// </summary>
        public const string Email = "updated_" + AccountExample.Email;

        /// <summary>
        /// The first name.
        /// </summary>
        public const string FirstName = "Updated " + AccountExample.FirstName;

        /// <summary>
        /// The last name.
        /// </summary>
        public const string LastName = "Updated " + AccountExample.LastName;

        /// <summary>
        /// The phone number.
        /// </summary>
        public const string PhoneNumber = "0011223344";

        /// <summary>
        /// The password.
        /// </summary>
        public const string Password = ChangePasswordRequestExample.NewPassword;

        /// <summary>
        /// The about.
        /// </summary>
        public const string About = "About";
    }

    /// <summary>
    /// Change password request example.
    /// </summary>
    public static class ChangePasswordRequestExample
    {
        /// <summary>
        /// Creates new password.
        /// </summary>
        public const string NewPassword = "New" + AccountExample.Password;
    }

    /// <summary>
    /// Search parameters request example.
    /// </summary>
    public static class SearchParametersRequestExample
    {
        /// <summary>
        /// The search.
        /// </summary>
        public const string Search = "Some search value";
    }

    /// <summary>
    /// Sort parameters request example.
    /// </summary>
    public static class SortParametersRequestExample
    {
        /// <summary>
        /// The order by.
        /// </summary>
        public const string OrderBy = "Title";

        /// <summary>
        /// The sort by.
        /// </summary>
        public const string SortBy = "Title";

        /// <summary>
        /// The current page.
        /// </summary>
        public const int CurrentPage = 1;

        /// <summary>
        /// The page size.
        /// </summary>
        public const int PageSize = 10;
    }

    /// <summary>
    /// Comment response example.
    /// </summary>
    public static class CommentResponseExample
    {
        /// <summary>
        /// The comment body.
        /// </summary>
        public const string CommentBody = "Comment response.";
    }

    /// <summary>
    /// Post view response example.
    /// </summary>
    public static class PostViewResponseExample
    {
        /// <summary>
        /// The title.
        /// </summary>
        public const string Title = "Post view response";

        /// <summary>
        /// The description.
        /// </summary>
        public const string Description = "Post view response.";

        /// <summary>
        /// The content.
        /// </summary>
        public const string Content = "Post view response.";

        /// <summary>
        /// The image URL.
        /// </summary>
        public const string ImageUrl = "ImageUrl";
    }

    /// <summary>
    /// Tag response example.
    /// </summary>
    public static class TagResponseExample
    {
        /// <summary>
        /// The title.
        /// </summary>
        public const string Title = "Tag response";
    }

    /// <summary>
    /// Profile response example.
    /// </summary>
    public static class ProfileResponseExample
    {
        /// <summary>
        /// The about.
        /// </summary>
        public const string About = "Profile response example";

        /// <summary>
        /// The profile img.
        /// </summary>
        public const string ProfileImg = "ProfileImg";
    }

    /// <summary>
    /// Message response example.
    /// </summary>
    public static class MessageResponseExample
    {
        /// <summary>
        /// The subject.
        /// </summary>
        public const string Subject = "Message response example.";

        /// <summary>
        /// The body.
        /// </summary>
        public const string Body = "Message response example.";
    }
}