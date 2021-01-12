namespace AuthService.Core.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// File provider,
    /// </summary>
    /// <seealso cref="PhysicalFileProvider" />
    /// <seealso cref="IShareFileProvider" />
    public class FileProvider : PhysicalFileProvider, IShareFileProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileProvider"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        [Obsolete]
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

        #region Utilities

        /// <summary>
        /// Deletes the directory recursive.
        /// </summary>
        /// <param name="path">The path.</param>
        private static void DeleteDirectoryRecursive(string path)
        {
            Directory.Delete(path, true);
            const int maxIterationToWait = 10;
            var curIteration = 0;

            //according to the documentation(https://msdn.microsoft.com/ru-ru/library/windows/desktop/aa365488.aspx) 
            //System.IO.Directory.Delete method ultimately (after removing the files) calls native 
            //RemoveDirectory function which marks the directory as "deleted". That's why we wait until 
            //the directory is actually deleted. For more details see https://stackoverflow.com/a/4245121
            while (Directory.Exists(path))
            {
                curIteration += 1;
                if (curIteration > maxIterationToWait)
                    return;
                Thread.Sleep(100);
            }
        }

        #endregion

        /// <summary>
        /// Combines the specified paths.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns>
        /// string.
        /// </returns>
        public virtual string Combine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        public virtual void CreateDirectory(string path)
        {
            if (!this.DirectoryExists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="path">The path.</param>
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

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public virtual void DeleteFile(string filePath)
        {
            if (!this.FileExists(filePath))
            {
                return;
            }

            File.Delete(filePath);
        }

        /// <summary>
        /// Directories the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// bool.
        /// </returns>
        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Directories the move.
        /// </summary>
        /// <param name="sourceDirName">Name of the source dir.</param>
        /// <param name="destDirName">Name of the dest dir.</param>
        public virtual void DirectoryMove(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }

        /// <summary>
        /// Enumerates the files.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="topDirectoryOnly">if set to <c>true</c> [top directory only].</param>
        /// <returns>
        /// IEnumerable.
        /// </returns>
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

        /// <summary>
        /// Files the copy.
        /// </summary>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <param name="destFileName">Name of the dest file.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        public virtual void FileCopy(string sourceFileName, string destFileName, bool overwrite = false)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        /// <summary>
        /// Files the exists.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>
        /// bool.
        /// </returns>
        public virtual bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// Files the length.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// long.
        /// </returns>
        public virtual long FileLength(string path)
        {
            if (!this.FileExists(path))
            {
                return -1;
            }

            return new FileInfo(path).Length;
        }

        /// <summary>
        /// Files the move.
        /// </summary>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <param name="destFileName">Name of the dest file.</param>
        public virtual void FileMove(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        /// <summary>
        /// Gets the absolute path.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns>
        /// string.
        /// </returns>
        public virtual string GetAbsolutePath(params string[] paths)
        {
            var allPaths = paths.ToList();
            allPaths.Insert(0, this.Root);

            return Path.Combine(allPaths.ToArray());
        }

        /// <summary>
        /// Gets the creation time.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// DateTime.
        /// </returns>
        public virtual DateTime GetCreationTime(string path)
        {
            return File.GetCreationTime(path);
        }

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="topDirectoryOnly">if set to <c>true</c> [top directory only].</param>
        /// <returns>
        /// string[].
        /// </returns>
        public virtual string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true)
        {
            if (string.IsNullOrEmpty(searchPattern))
            {
                searchPattern = "*";
            }

            return
                Directory.GetDirectories(
                    path,
                    searchPattern,
                    topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// string.
        /// </returns>
        public virtual string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Gets the directory name only.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// string.
        /// </returns>
        public virtual string GetDirectoryNameOnly(string path)
        {
            return new DirectoryInfo(path).Name;
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>
        /// string.
        /// </returns>
        public virtual string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// string.
        /// </returns>
        public virtual string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>
        /// string.
        /// </returns>
        public virtual string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="topDirectoryOnly">if set to <c>true</c> [top directory only].</param>
        /// <returns>
        /// string[].
        /// </returns>
        public virtual string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true)
        {
            if (string.IsNullOrEmpty(searchPattern))
            {
                searchPattern = "*.*";
            }

            return
                Directory.GetFiles(
                    directoryPath,
                    searchPattern,
                    topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /// <summary>
        /// Gets the last access time.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// DateTime.
        /// </returns>
        public virtual DateTime GetLastAccessTime(string path)
        {
            return File.GetLastAccessTime(path);
        }

        /// <summary>
        /// Gets the last write time.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// DateTime.
        /// </returns>
        public virtual DateTime GetLastWriteTime(string path)
        {
            return File.GetLastWriteTime(path);
        }

        /// <summary>
        /// Gets the last write time UTC.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// DateTime.
        /// </returns>
        public virtual DateTime GetLastWriteTimeUtc(string path)
        {
            return File.GetLastWriteTimeUtc(path);
        }

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>
        /// string.
        /// </returns>
        public virtual string GetParentDirectory(string directoryPath)
        {
            return Directory.GetParent(directoryPath).FullName;
        }

        /// <summary>
        /// Determines whether the specified path is directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is directory; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsDirectory(string path)
        {
            return this.DirectoryExists(path);
        }

        /// <summary>
        /// Maps the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// string.
        /// </returns>
        public virtual string MapPath(string path)
        {
            path = path.Replace("~/", string.Empty).TrimStart('/').Replace('/', '\\');
            return Path.Combine(this.BaseDirectory ?? string.Empty, path);
        }

        /// <summary>
        /// Reads all bytes.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>
        /// byte[].
        /// </returns>
        public virtual byte[] ReadAllBytes(string filePath)
        {
            return File.Exists(filePath) ? File.ReadAllBytes(filePath) : new byte[0];
        }

        /// <summary>
        /// Reads all text.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// string.
        /// </returns>
        public virtual string ReadAllText(string path, Encoding encoding)
        {
            return File.ReadAllText(path, encoding);
        }

        /// <summary>
        /// Sets the last write time UTC.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="lastWriteTimeUtc">The last write time UTC.</param>
        public virtual void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
        }

        /// <summary>
        /// Writes all bytes.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="bytes">The bytes.</param>
        public virtual void WriteAllBytes(string filePath, byte[] bytes)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// Writes all text.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="encoding">The encoding.</param>
        public virtual void WriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents, encoding);
        }

        /// <summary>
        /// Gets the base directory.
        /// </summary>
        /// <value>
        /// The base directory.
        /// </value>
        protected string BaseDirectory { get; }
    }
}
