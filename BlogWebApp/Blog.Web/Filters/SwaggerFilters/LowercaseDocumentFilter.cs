namespace Blog.Web.Filters.SwaggerFilters
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Linq;

    /// <summary>
    /// Lowercase routes filter
    /// </summary>
    public class LowercaseDocumentFilter : IDocumentFilter
    {
        /// <inheritdoc />
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths = (OpenApiPaths)swaggerDoc.Paths.ToDictionary(entry => LowercaseEverythingButParameters(entry.Key),
                entry => entry.Value);
        }

        /// <summary>
        /// Lowercase the everything but parameters.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>string.</returns>
        private static string LowercaseEverythingButParameters(string key)
        {
            return string.Join('/', key.Split('/').Select(x => x.Contains("{") ? x : x.ToLower()));
        }
    }
}
