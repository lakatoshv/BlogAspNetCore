namespace Blog.FSharp.Core.Helpers

open System
open System.Threading
open System.Threading.Tasks

/// <summary>
/// Run Asynchronous methods as Synchronous.
/// </summary>
[<AbstractClass; Sealed>]
type AsyncHelper private () =

    /// <summary>
    /// The application task factory.
    /// </summary>
    static let appTaskFactory =
        new TaskFactory(
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default)

    /// <summary>
    /// Runs synchronize.
    /// </summary>
    static member RunSync<'TResult>(func: Func<Task<'TResult>>) : 'TResult =
        appTaskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult()

    /// <summary>
    /// Runs synchronize.
    /// </summary>
    static member RunSync(func: Func<Task>) : unit =
        appTaskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult()