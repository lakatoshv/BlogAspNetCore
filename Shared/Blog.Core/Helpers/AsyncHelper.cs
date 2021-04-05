// <copyright file="AsyncHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Helpers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Run Asynchronous methods as Synchronous.
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// The application task factory.
        /// </summary>
        private static readonly TaskFactory AppTaskFactory = new
            TaskFactory(
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

        /// <summary>
        /// Runs the synchronize.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AppTaskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Runs the synchronize.
        /// </summary>
        /// <param name="func">The function.</param>
        public static void RunSync(Func<Task> func)
        {
            AppTaskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }
    }
}