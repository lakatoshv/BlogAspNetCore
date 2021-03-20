namespace Blog.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Calabonga.Microservices.Core.Extensions;

    /// <summary>
    /// Email validation helper.
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// Validates emails entries.
        /// </summary>
        /// <param name="emails">emails.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable<string> GetValidEmails(string emails)
        {
            if (string.IsNullOrWhiteSpace(emails))
            {
                return null;
            }
            var split = emails.Split(new[] { ';', '|', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return split.Where(x => x.IsEmail());
        }
    }
}