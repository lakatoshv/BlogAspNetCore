// <copyright file="IEmailService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.EmailServices.Interfaces
{
    using System.Threading.Tasks;
    using Blog.Core.Emails;

    /// <summary>
    /// Email service interface.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="email">email.</param>
        void Send(Email email);

        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="body">body.</param>
        /// <param name="subject">subject.</param>
        /// <param name="from">from.</param>
        /// <param name="to">to.</param>
        void Send(string body, string subject, string from, string to);

        /// <summary>
        /// Async send email.
        /// </summary>
        /// <param name="email">email.</param>
        /// <returns>Task.</returns>
        Task SendAsync(Email email);

        /// <summary>
        /// Async send email.
        /// </summary>
        /// <param name="body">body.</param>
        /// <param name="subject">subject.</param>
        /// <param name="from">from.</param>
        /// <param name="to">to.</param>
        /// <returns>Task.</returns>
        Task SendAsync(string body, string subject, string from, string to);
    }
}
