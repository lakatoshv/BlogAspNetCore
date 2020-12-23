// <copyright file="IEmailTemplateProvider.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Email.Templates
{
    /// <summary>
    /// Email template provider interface.
    /// </summary>
    public interface IEmailTemplateProvider
    {
        /// <summary>
        /// Resolve body.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="templateType">templateType.</param>
        /// <param name="model">model.</param>
        /// <returns>string.</returns>
        string ResolveBody<T>(TemplateTypes templateType, T model);
    }
}
