// <copyright file="Utilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Helpers;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// System utilities.
/// </summary>
public static class Utilities
{
    /// <summary>
    /// Computes the hash.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <returns>byte[].</returns>
    /// <exception cref="FileNotFoundException">File not found exception.</exception>
    public static byte[] ComputeHash(string filePath)
    {
        var runCount = 1;
        while (runCount < 4)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException();
                }

                using var fs = File.OpenRead(filePath);

                return SHA1.Create().ComputeHash(fs);
            }
            catch (IOException ex)
            {
                if (runCount == 3 || ex.HResult != -2147024864)
                {
                    throw;
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(2, runCount)));
                    runCount++;
                }
            }
        }

        return new byte[20];
    }

    /// <summary>
    /// Gets the content of the file.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <returns>Task.</returns>
    /// <exception cref="FileNotFoundException">File not found exception.</exception>
    public static async Task<string> GetFileContent(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            return await File.ReadAllTextAsync(filePath);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Remove file from directory (physical deleting).
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <exception cref="FileNotFoundException">File not found exception.</exception>
    public static void DeleteFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            File.Delete(filePath);
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    /// Sets the content of the file.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="content">The content.</param>
    public static async Task SetFileContent(string filePath, string content)
    {
        try
        {
            var folder = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (string.IsNullOrEmpty(filePath) || File.Exists(filePath))
            {
            }

            await using var fs = File.Create(filePath);

            var info = new UTF8Encoding(true).GetBytes(content);
            await fs.WriteAsync(info, 0, info.Length);
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    /// Generate ETag for content bytes.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="contentBytes">The content bytes.</param>
    /// <returns>string.</returns>
    public static string GetETag(string key, byte[] contentBytes)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var combinedBytes = Combine(keyBytes, contentBytes);

        return GenerateETag(combinedBytes);
    }

    /// <summary>
    /// Gets the working folder.
    /// </summary>
    /// <returns>string.</returns>
    public static string GetWorkingFolder()
    {
        var location = System.Reflection.Assembly.GetEntryAssembly()?.Location;

        return Path.GetDirectoryName(location);
    }

    /// <summary>
    /// Generates the e tag.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>string.</returns>
    private static string GenerateETag(byte[] data)
    {
        using var md5 = MD5.Create();

        var hash = md5.ComputeHash(data);
        var hex = BitConverter.ToString(hash);

        return hex.Replace("-", string.Empty);
    }

    /// <summary>
    /// Combines the specified a.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns>byte[].</returns>
    private static byte[] Combine(byte[] a, byte[] b)
    {
        var c = new byte[a.Length + b.Length];
        Buffer.BlockCopy(a, 0, c, 0, a.Length);
        Buffer.BlockCopy(b, 0, c, a.Length, b.Length);

        return c;
    }
}