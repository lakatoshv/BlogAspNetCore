namespace Blog.FSharp.Core.Infrastructure

open System
open System.IO
open System.Text
open System.Threading
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.FileProviders

type FileProvider(hostingEnvironment: IHostingEnvironment) =
    inherit PhysicalFileProvider(
        if String.IsNullOrEmpty(hostingEnvironment.WebRootPath) then hostingEnvironment.ContentRootPath
        else hostingEnvironment.WebRootPath
    )

    let baseDirectory =
        if String.IsNullOrEmpty(hostingEnvironment.ContentRootPath) then String.Empty
        else hostingEnvironment.ContentRootPath

    member _.BaseDirectory = baseDirectory

    interface IShareFileProvider with
        member _.Combine(paths: string[]) =
            Path.Combine(paths)

        member _.CreateDirectory(path: string) =
            if not (Directory.Exists(path)) then
                Directory.CreateDirectory(path) |> ignore

        member _.CreateFile(path: string) =
            if not (File.Exists(path)) then
                use _ = File.Create(path)
                ()

        member this.DeleteDirectory(path: string) =
            if String.IsNullOrEmpty(path) then
                raise (ArgumentNullException("path"))

            let provider = this :> IShareFileProvider

            for dir in Directory.GetDirectories(path) do
                provider.DeleteDirectory(dir)

            let rec deleteDirRecursive (p: string) maxRetry =
                let mutable retry = 0
                while Directory.Exists(p) && retry < maxRetry do
                    try
                        Directory.Delete(p, true)
                    with
                    | :? IOException
                    | :? UnauthorizedAccessException ->
                        Thread.Sleep(100)
                        retry <- retry + 1

            deleteDirRecursive path 10

        member _.DeleteFile(path: string) =
            if File.Exists(path) then File.Delete(path)

        member _.DirectoryExists(path: string) = Directory.Exists(path)

        member _.FileExists(path: string) = File.Exists(path)

        member _.FileLength(path: string) =
            if File.Exists(path) then FileInfo(path).Length else -1L

        member _.MapPath(path: string) =
            path.Replace("~/", "").TrimStart('/').Replace('/', Path.DirectorySeparatorChar)
            |> fun p -> Path.Combine(baseDirectory, p)

        member _.ReadAllText(path: string, encoding: Encoding) =
            File.ReadAllText(path, encoding)

        member _.WriteAllText(path: string, contents: string, encoding: Encoding) =
            File.WriteAllText(path, contents, encoding)