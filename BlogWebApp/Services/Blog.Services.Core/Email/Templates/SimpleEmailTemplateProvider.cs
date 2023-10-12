// <copyright file="SimpleEmailTemplateProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Email.Templates;

using System.Collections.Generic;

/// <summary>
/// Simple email template provider.
/// </summary>
public class SimpleEmailTemplateProvider : IEmailTemplateProvider
{
    /// <summary>
    /// Dictionary.
    /// </summary>
    private readonly Dictionary<TemplateTypes, string> templates = new()
    {
        { TemplateTypes.EmailVerification, "Please confirm your account by clicking <a href='{{BaseUrl}}account/confirm-email?email={{email}}&token={{token}}'>this link</a>" },
        { TemplateTypes.PasswordRestore, "Please reset your password by clicking <a href='{{BaseUrl}}reset-password?email={{email}}&token={{token}}'>this link</a>" },
    };

    // TODO Need rewrite this as plugin

    /// <inheritdoc cref="IEmailTemplateProvider"/>
    public string ResolveBody<T>(TemplateTypes templateType, T model)
    {
        var properties = typeof(T).GetProperties();
        var template = this.templates[templateType];

        foreach (var property in properties)
        {
            var key = "{{" + property.Name + "}}";
            var value = property.GetValue(model).ToString();

            template = template.Replace(key, value);
        }

        return template;
    }
}