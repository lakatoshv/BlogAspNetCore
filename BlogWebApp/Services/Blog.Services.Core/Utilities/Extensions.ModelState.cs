// <copyright file="Extensions.ModelState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Utilities
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    /// <summary>
    /// ModelState extensions.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Add errors.
        /// </summary>
        /// <param name="modelState">modelState.</param>
        /// <param name="identityResult">identityResult.</param>
        /// <returns>ModelStateDictionary.</returns>
        public static ModelStateDictionary AddErrors(this ModelStateDictionary modelState, IdentityResult identityResult)
        {
            foreach (var e in identityResult.Errors)
            {
                modelState.TryAddModelError(e.Code, e.Description);
            }

            return modelState;
        }

        /// <summary>
        /// Add error.
        /// </summary>
        /// <param name="modelState">modelState.</param>
        /// <param name="code">code.</param>
        /// <param name="description">description.</param>
        /// <returns>ModelStateDictionary.</returns>
        public static ModelStateDictionary AddError(this ModelStateDictionary modelState, string code, string description)
        {
            modelState.TryAddModelError(code, description);
            return modelState;
        }

        /// <summary>
        /// Add application error.
        /// </summary>
        /// <param name="response">response.</param>
        /// <param name="message">message.</param>
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);

            // CORS
            response.Headers.Add("access-control-expose-headers", "Application-Error");
        }

        /// <summary>
        /// Add model error.
        /// </summary>
        /// <param name="modelState">modelState.</param>
        /// <param name="errors">errors.</param>
        public static void AddModelError(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            foreach (var err in errors)
            {
                modelState.AddModelError(err.Code, err.Description);
            }
        }
    }
}
