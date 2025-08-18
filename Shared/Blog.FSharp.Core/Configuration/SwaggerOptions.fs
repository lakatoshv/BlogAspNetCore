namespace Blog.Core.Configuration

/// Swagger options.
type SwaggerOptions = {
    /// The JSON route.
    JsonRoute: string option

    /// The description.
    Description: string option

    /// The UI endpoint.
    UiEndpoint: string option
}
