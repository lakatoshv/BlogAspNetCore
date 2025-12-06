// <copyright file="FileProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

/// <summary>
/// File provider.
/// </summary>
public class FileProvider : PhysicalFileProvider, IShareFileProvider
{
    /// <summary>
    /// Gets base directory.
    /// </summary>
    protected string BaseDirectory { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileProvider"/> class.
    /// </summary>
    /// <param name="hostingEnvironment">hostingEnvironment.</param>
    public FileProvider(IHostingEnvironment hostingEnvironment)
        : base(File.Exists(hostingEnvironment.WebRootPath) ? Path.GetDirectoryName(hostingEnvironment.WebRootPath) : hostingEnvironment.WebRootPath)
    {
        var path = hostingEnvironment.ContentRootPath ?? string.Empty;
        if (File.Exists(path))
        {
            path = Path.GetDirectoryName(path);
        }

        this.BaseDirectory = path;
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string Combine(params string[] paths)
    {
        return Path.Combine(paths);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual void CreateDirectory(string path)
    {
        if (!this.DirectoryExists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual void CreateFile(string path)
    {
        if (this.FileExists(path))
        {
            return;
        }

        // we use 'using' to close the file after it's created
        using (File.Create(path))
        {
        }
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public void DeleteDirectory(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        // find more info about directory deletion
        // and why we use this approach at https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true
        foreach (var directory in Directory.GetDirectories(path))
        {
            this.DeleteDirectory(directory);
        }

        try
        {
            DeleteDirectoryRecursive(path);
        }
        catch (IOException)
        {
            DeleteDirectoryRecursive(path);
        }
        catch (UnauthorizedAccessException)
        {
            DeleteDirectoryRecursive(path);
        }
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual void DeleteFile(string filePath)
    {
        if (!this.FileExists(filePath))
        {
            return;
        }

        File.Delete(filePath);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual void DirectoryMove(string sourceDirName, string destDirName)
    {
        Directory.Move(sourceDirName, destDirName);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual IEnumerable<string> EnumerateFiles(
        string directoryPath,
        string searchPattern,
        bool topDirectoryOnly = true)
    {
        return Directory.EnumerateFiles(
            directoryPath,
            searchPattern,
            topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual void FileCopy(string sourceFileName, string destFileName, bool overwrite = false)
    {
        File.Copy(sourceFileName, destFileName, overwrite);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual long FileLength(string path)
    {
        if (!this.FileExists(path))
        {
            return -1;
        }

        return new FileInfo(path).Length;
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual void FileMove(string sourceFileName, string destFileName)
    {
        File.Move(sourceFileName, destFileName);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string GetAbsolutePath(params string[] paths)
    {
        var allPaths = paths.ToList();
        allPaths.Insert(0, this.Root);

        return Path.Combine(allPaths.ToArray());
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual DateTime GetCreationTime(string path)
    {
        return File.GetCreationTime(path);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true)
    {
        if (string.IsNullOrEmpty(searchPattern))
        {
            searchPattern = "*";
        }

        return Directory.GetDirectories(
            path,
            searchPattern,
            topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string GetDirectoryName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string GetDirectoryNameOnly(string path)
    {
        return new DirectoryInfo(path).Name;
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string GetFileExtension(string filePath)
    {
        return Path.GetExtension(filePath);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string GetFileName(string path)
    {
        return Path.GetFileName(path);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string GetFileNameWithoutExtension(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true)
    {
        if (string.IsNullOrEmpty(searchPattern))
        {
            searchPattern = "*.*";
        }

        return Directory.GetFiles(
            directoryPath,
            searchPattern,
            topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual DateTime GetLastAccessTime(string path)
    {
        return File.GetLastAccessTime(path);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual DateTime GetLastWriteTime(string path)
    {
        return File.GetLastWriteTime(path);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual DateTime GetLastWriteTimeUtc(string path)
    {
        return File.GetLastWriteTimeUtc(path);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string GetParentDirectory(string directoryPath)
    {
        return Directory.GetParent(directoryPath)?.FullName;
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual bool IsDirectory(string path)
    {
        return this.DirectoryExists(path);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string MapPath(string path)
    {
        path = path.Replace("~/", string.Empty).TrimStart('/').Replace('/', '\\');
        return Path.Combine(this.BaseDirectory ?? string.Empty, path);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual byte[] ReadAllBytes(string filePath)
    {
        return File.Exists(filePath) ? File.ReadAllBytes(filePath) : new byte[0];
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual string ReadAllText(string path, Encoding encoding)
    {
        return File.ReadAllText(path, encoding);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
    {
        File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual void WriteAllBytes(string filePath, byte[] bytes)
    {
        File.WriteAllBytes(filePath, bytes);
    }

    /// <inheritdoc cref="IServiceProvider"/>
    public virtual void WriteAllText(string path, string contents, Encoding encoding)
    {
        File.WriteAllText(path, contents, encoding);
    }

    /// <summary>
    /// Delete directory recursive.
    /// </summary>
    /// <param name="path">path.</param>
    private static void DeleteDirectoryRecursive(string path)
    {
        Directory.Delete(path, true);
        const int maxIterationToWait = 10;
        var curIteration = 0;

        // according to the documentation(https://msdn.microsoft.com/ru-ru/library/windows/desktop/aa365488.aspx)
        // System.IO.Directory.Delete method ultimately (after removing the files) calls native
        // RemoveDirectory function which marks the directory as "deleted". That's why we wait until
        // the directory is actually deleted. For more details see https://stackoverflow.com/a/4245121
        while (Directory.Exists(path))
        {
            curIteration += 1;
            if (curIteration > maxIterationToWait)
            {
                return;
            }

            Thread.Sleep(100);
        }
    }
}