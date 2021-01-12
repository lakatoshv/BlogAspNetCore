// <copyright file="IShareFileProvider.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Share file provider.
    /// </summary>
    public interface IShareFileProvider : IFileProvider
    {
        /// <summary>
        /// Combine.
        /// </summary>
        /// <param name="paths">paths.</param>
        /// <returns>string.</returns>
        string Combine(params string[] paths);

        /// <summary>
        /// Create directory.
        /// </summary>
        /// <param name="path">path.</param>
        void CreateDirectory(string path);

        /// <summary>
        /// Create file.
        /// </summary>
        /// <param name="path">path.</param>
        void CreateFile(string path);

        /// <summary>
        /// Delete directory.
        /// </summary>
        /// <param name="path">path.</param>
        void DeleteDirectory(string path);

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="filePath">filePath.</param>
        void DeleteFile(string filePath);

        /// <summary>
        /// Directory exists.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>bool.</returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Directory move.
        /// </summary>
        /// <param name="sourceDirName">sourceDirName.</param>
        /// <param name="destDirName">destDirName.</param>
        void DirectoryMove(string sourceDirName, string destDirName);

        /// <summary>
        /// Enumerate files.
        /// </summary>
        /// <param name="directoryPath">directoryPath.</param>
        /// <param name="searchPattern">searchPattern.</param>
        /// <param name="topDirectoryOnly">topDirectoryOnly.</param>
        /// <returns>IEnumerable.</returns>
        IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern, bool topDirectoryOnly = true);

        /// <summary>
        /// File copy.
        /// </summary>
        /// <param name="sourceFileName">sourceFileName.</param>
        /// <param name="destFileName">destFileName.</param>
        /// <param name="overwrite">overwrite.</param>
        void FileCopy(string sourceFileName, string destFileName, bool overwrite = false);

        /// <summary>
        /// File exists.
        /// </summary>
        /// <param name="filePath">filePath.</param>
        /// <returns>bool.</returns>
        bool FileExists(string filePath);

        /// <summary>
        /// File length.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>long.</returns>
        long FileLength(string path);

        /// <summary>
        /// File move.
        /// </summary>
        /// <param name="sourceFileName">sourceFileName.</param>
        /// <param name="destFileName">destFileName.</param>
        void FileMove(string sourceFileName, string destFileName);

        /// <summary>
        /// Get absolute path.
        /// </summary>
        /// <param name="paths">paths.</param>
        /// <returns>string.</returns>
        string GetAbsolutePath(params string[] paths);

        // DirectorySecurity GetAccessControl(string path);

        /// <summary>
        /// Get creation time.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>DateTime.</returns>
        DateTime GetCreationTime(string path);

        /// <summary>
        /// Get directories.
        /// </summary>
        /// <param name="path">path.</param>
        /// <param name="searchPattern">searchPattern.</param>
        /// <param name="topDirectoryOnly">topDirectoryOnly.</param>
        /// <returns>string[].</returns>
        string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true);

        /// <summary>
        /// Get directory name.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>string.</returns>
        string GetDirectoryName(string path);

        /// <summary>
        /// Get directory name only.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>string.</returns>
        string GetDirectoryNameOnly(string path);

        /// <summary>
        /// Get file extension.
        /// </summary>
        /// <param name="filePath">filePath.</param>
        /// <returns>string.</returns>
        string GetFileExtension(string filePath);

        /// <summary>
        /// Get file name.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>string.</returns>
        string GetFileName(string path);

        /// <summary>
        /// Get file name without extension.
        /// </summary>
        /// <param name="filePath">filePath.</param>
        /// <returns>string.</returns>
        string GetFileNameWithoutExtension(string filePath);

        /// <summary>
        /// Get files.
        /// </summary>
        /// <param name="directoryPath">directoryPath.</param>
        /// <param name="searchPattern">searchPattern.</param>
        /// <param name="topDirectoryOnly">topDirectoryOnly.</param>
        /// <returns>string[].</returns>
        string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true);

        /// <summary>
        /// Get last access time.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>DateTime.</returns>
        DateTime GetLastAccessTime(string path);

        /// <summary>
        /// Get last write time.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>DateTime.</returns>
        DateTime GetLastWriteTime(string path);

        /// <summary>
        /// Get last write time utc.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>DateTime.</returns>
        DateTime GetLastWriteTimeUtc(string path);

        /// <summary>
        /// Get parent directory.
        /// </summary>
        /// <param name="directoryPath">directoryPath.</param>
        /// <returns>string.</returns>
        string GetParentDirectory(string directoryPath);

        /// <summary>
        /// Is directory.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>bool.</returns>
        bool IsDirectory(string path);

        /// <summary>
        /// Map path.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>string.</returns>
        string MapPath(string path);

        /// <summary>
        /// Read all bytes.
        /// </summary>
        /// <param name="filePath">filePath.</param>
        /// <returns>byte[].</returns>
        byte[] ReadAllBytes(string filePath);

        /// <summary>
        /// Read all text.
        /// </summary>
        /// <param name="path">path.</param>
        /// <param name="encoding">encoding.</param>
        /// <returns>string.</returns>
        string ReadAllText(string path, Encoding encoding);

        /// <summary>
        /// Set last write time utc.
        /// </summary>
        /// <param name="path">path.</param>
        /// <param name="lastWriteTimeUtc">lastWriteTimeUtc.</param>
        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <summary>
        /// Write all bytes.
        /// </summary>
        /// <param name="filePath">filePath.</param>
        /// <param name="bytes">bytes.</param>
        void WriteAllBytes(string filePath, byte[] bytes);

        /// <summary>
        /// Write all text.
        /// </summary>
        /// <param name="path">path.</param>
        /// <param name="contents">contents.</param>
        /// <param name="encoding">encoding.</param>
        void WriteAllText(string path, string contents, Encoding encoding);
    }
}
