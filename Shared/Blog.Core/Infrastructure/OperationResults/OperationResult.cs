// <copyright file="OperationResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.OperationResults;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Operation result.
/// </summary>
[Serializable]
public abstract class OperationResult
{
    /// <summary>
    /// The logs.
    /// </summary>
    private readonly IList<string> _logs = new List<string>();

    /// <summary>
    /// Initializes a new instance of the <see cref="OperationResult"/> class.
    /// </summary>
    protected OperationResult() => this.ActivityId = Generate(11);

    /// <summary>
    /// Gets or sets the activity identifier.
    /// </summary>
    /// <value>
    /// The activity identifier.
    /// </value>
    public string ActivityId { get; set; }

    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    /// <value>
    /// The metadata.
    /// </value>
    public Metadata Metadata { get; set; }

    /// <summary>
    /// Gets or sets the exception.
    /// </summary>
    /// <value>
    /// The exception.
    /// </value>
    public Exception Exception { get; set; }

    /// <summary>
    /// Gets the logs.
    /// </summary>
    /// <value>
    /// The logs.
    /// </value>
    public IEnumerable<string> Logs => this._logs;

    /// <summary>
    /// Creates the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="error">The error.</param>
    /// <returns>The TResult.</returns>
    public static OperationResult<TResult> CreateResult<TResult>(
        TResult result,
        Exception error)
        => new()
        {
            Result = result,
        };

    /// <summary>
    /// Creates the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="result">The result.</param>
    /// <returns>OperationResult.</returns>
    public static OperationResult<TResult> CreateResult<TResult>(TResult result) => CreateResult(result, null);

    /// <summary>
    /// Creates the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <returns>OperationResult.</returns>
    public static OperationResult<TResult> CreateResult<TResult>() => CreateResult<TResult>(default, null);

    /// <summary>
    /// Appends the log.
    /// </summary>
    /// <param name="messageLog">The message log.</param>
    public void AppendLog(string messageLog)
    {
        if (string.IsNullOrEmpty(messageLog))
        {
            return;
        }

        if (messageLog.Length > 500)
        {
            this._logs.Add(messageLog[..500]);
        }

        this._logs.Add(messageLog);
    }

    /// <summary>
    /// Appends the log.
    /// </summary>
    /// <param name="messageLogs">The message logs.</param>
    public void AppendLog(IEnumerable<string> messageLogs)
    {
        if (messageLogs == null)
        {
            return;
        }

        foreach (var messageLog in messageLogs)
        {
            this.AppendLog(messageLog);
        }
    }

    /// <summary>
    /// Generates the specified size.
    /// </summary>
    /// <param name="size">The size.</param>
    /// <returns>string.</returns>
    private static string Generate(int size)
    {
        var charArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        var data = new byte[1];
        using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
        {
            cryptoServiceProvider.GetNonZeroBytes(data);
            data = new byte[size];
            cryptoServiceProvider.GetNonZeroBytes(data);
        }

        var stringBuilder = new StringBuilder(size);
        foreach (var num in data)
        {
            stringBuilder.Append(charArray[num % charArray.Length]);
        }

        return stringBuilder.ToString();
    }
}