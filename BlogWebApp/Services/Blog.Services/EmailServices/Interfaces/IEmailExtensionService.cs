// <copyright file="IEmailExtensionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.EmailServices.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Email extension service interface.
    /// </summary>
    public interface IEmailExtensionService
    {
        /// <summary>
        /// Async send verification email.
        /// </summary>
        /// <param name="email">email.</param>
        /// <param name="token">token.</param>
        /// <returns>Task.</returns>
        Task SendVerificationEmailAsync(string email, string token);

        /// <summary>
        /// Async send password reset email.
        /// </summary>
        /// <param name="email">email.</param>
        /// <param name="token">token.</param>
        /// <returns>Task.</returns>
        Task SendPasswordResetEmailAsync(string email, string token);
    }
}
