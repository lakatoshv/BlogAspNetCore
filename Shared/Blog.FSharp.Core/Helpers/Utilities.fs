namespace Blog.FSharp.Core.Helpers

open System
open System.IO
open System.Security.Cryptography
open System.Text
open System.Threading
open System.Threading.Tasks

/// <summary>
/// System utilities.
/// </summary>
module Utilities =

    /// <summary>
    /// Computes the SHA1 hash of a file, retrying up to 3 times in case of IO exceptions.
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <returns>Byte array of the hash.</returns>
    let computeHash (filePath: string) : byte[] =
        let mutable runCount = 1
        let mutable result: byte[] = null
        while runCount < 4 do
            try
                if not (File.Exists(filePath)) then
                    raise (FileNotFoundException())
                
                use fs = File.OpenRead(filePath)
                result <- SHA1.Create().ComputeHash(fs)
                runCount <- 4 // exit loop
            with
            | :? IOException as ex ->
                if runCount = 3 || ex.HResult <> -2147024864 then
                    raise ex
                else
                    Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(2.0, float runCount)))
                    runCount <- runCount + 1
        if isNull result then Array.zeroCreate 20 else result

    /// <summary>
    /// Reads the content of a file asynchronously.
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <returns>File content as string, or null if exception occurs.</returns>
    let getFileContent (filePath: string) : Task<string> = task {
        try
            if not (File.Exists(filePath)) then
                raise (FileNotFoundException())
            return! File.ReadAllTextAsync(filePath)
        with
        | _ -> return null
    }

    /// <summary>
    /// Deletes a file if it exists.
    /// </summary>
    /// <param name="filePath">File path.</param>
    let deleteFile (filePath: string) =
        try
            if not (File.Exists(filePath)) then
                raise (FileNotFoundException())
            File.Delete(filePath)
        with
        | _ -> () // ignore errors

    /// <summary>
    /// Writes content to a file asynchronously, creating folder if necessary.
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <param name="content">Content string.</param>
    let setFileContent (filePath: string) (content: string) : Task = task {
        try
            let folder = Path.GetDirectoryName(filePath)
            if not (Directory.Exists(folder)) then Directory.CreateDirectory(folder) |> ignore
            use fs = File.Create(filePath)
            let bytes = UTF8Encoding(true).GetBytes(content)
            do! fs.WriteAsync(bytes, 0, bytes.Length)
        with
        | _ -> () // ignore errors
    }

    /// <summary>
    /// Generates an ETag from key and content bytes.
    /// </summary>
    /// <param name="key">Key string.</param>
    /// <param name="contentBytes">Content bytes.</param>
    /// <returns>ETag string.</returns>
    let getETag (key: string) (contentBytes: byte[]) : string =
        let keyBytes = Encoding.UTF8.GetBytes(key)
        let combined = Array.append keyBytes contentBytes
        use md5 = MD5.Create()
        let hash = md5.ComputeHash(combined)
        BitConverter.ToString(hash).Replace("-", String.Empty)

    /// <summary>
    /// Gets the folder of the executing assembly.
    /// </summary>
    /// <returns>Folder path.</returns>
    /// <summary>
    let getWorkingFolder () =
        match System.Reflection.Assembly.GetEntryAssembly() with
        | null -> null
        | asm -> Path.GetDirectoryName(asm.Location)



