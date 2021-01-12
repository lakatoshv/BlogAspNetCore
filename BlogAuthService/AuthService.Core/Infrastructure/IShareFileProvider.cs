namespace AuthService.Core.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Share file provider interface.
    /// </summary>
    /// <seealso cref="IFileProvider" />
    public interface IShareFileProvider : IFileProvider
    {
        /// <summary>
        /// Combines the specified paths.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns>string.</returns>
        string Combine(params string[] paths);

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        void CreateDirectory(string path);

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="path">The path.</param>
        void CreateFile(string path);

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        void DeleteDirectory(string path);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void DeleteFile(string filePath);

        /// <summary>
        /// Directories the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>bool.</returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Directories the move.
        /// </summary>
        /// <param name="sourceDirName">Name of the source dir.</param>
        /// <param name="destDirName">Name of the dest dir.</param>
        void DirectoryMove(string sourceDirName, string destDirName);

        /// <summary>
        /// Enumerates the files.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="topDirectoryOnly">if set to <c>true</c> [top directory only].</param>
        /// <returns>IEnumerable.</returns>
        IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern, bool topDirectoryOnly = true);

        /// <summary>
        /// Files the copy.
        /// </summary>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <param name="destFileName">Name of the dest file.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        void FileCopy(string sourceFileName, string destFileName, bool overwrite = false);

        /// <summary>
        /// Files the exists.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>bool.</returns>
        bool FileExists(string filePath);

        /// <summary>
        /// Files the length.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>long.</returns>
        long FileLength(string path);

        /// <summary>
        /// Files the move.
        /// </summary>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <param name="destFileName">Name of the dest file.</param>
        void FileMove(string sourceFileName, string destFileName);

        /// <summary>
        /// Gets the absolute path.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns>string.</returns>
        string GetAbsolutePath(params string[] paths);

        // DirectorySecurity GetAccessControl(string path);

        /// <summary>
        /// Gets the creation time.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>DateTime.</returns>
        DateTime GetCreationTime(string path);

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="topDirectoryOnly">if set to <c>true</c> [top directory only].</param>
        /// <returns>string[].</returns>
        string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true);

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>string.</returns>
        string GetDirectoryName(string path);

        /// <summary>
        /// Gets the directory name only.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>string.</returns>
        string GetDirectoryNameOnly(string path);

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>string.</returns>
        string GetFileExtension(string filePath);

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>string.</returns>
        string GetFileName(string path);

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>string.</returns>
        string GetFileNameWithoutExtension(string filePath);

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="topDirectoryOnly">if set to <c>true</c> [top directory only].</param>
        /// <returns>string[].</returns>
        string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true);

        /// <summary>
        /// Gets the last access time.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>DateTime.</returns>
        DateTime GetLastAccessTime(string path);

        /// <summary>
        /// Gets the last write time.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>DateTime.</returns>
        DateTime GetLastWriteTime(string path);

        /// <summary>
        /// Gets the last write time UTC.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>DateTime.</returns>
        DateTime GetLastWriteTimeUtc(string path);

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>string.</returns>
        string GetParentDirectory(string directoryPath);

        /// <summary>
        /// Determines whether the specified path is directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is directory; otherwise, <c>false</c>.
        /// </returns>
        bool IsDirectory(string path);

        /// <summary>
        /// Maps the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>string.</returns>
        string MapPath(string path);

        /// <summary>
        /// Reads all bytes.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>byte[].</returns>
        byte[] ReadAllBytes(string filePath);

        /// <summary>
        /// Reads all text.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>string.</returns>
        string ReadAllText(string path, Encoding encoding);

        /// <summary>
        /// Sets the last write time UTC.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="lastWriteTimeUtc">The last write time UTC.</param>
        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <summary>
        /// Writes all bytes.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="bytes">The bytes.</param>
        void WriteAllBytes(string filePath, byte[] bytes);

        /// <summary>
        /// Writes all text.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="encoding">The encoding.</param>
        void WriteAllText(string path, string contents, Encoding encoding);
    }
}
