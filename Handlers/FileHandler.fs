module FsFs.Handlers.FileHandler

open System.IO
open Giraffe

open FsFs.Infrastructure.Config
open FsFs.Infrastructure.ResponseFactory

let private safeGetFiles (rootDir: string) (subPath: string) : Result<string[], string> =
    try
        Directory.GetFiles(Path.Combine(rootDir, "resource", subPath))
        |> Array.map (fun file -> Path.GetRelativePath(rootDir, file))
        |> Ok
    with ex ->
        Error ex.Message

let private safeGetAllFiles (rootDir: string) : Result<string[], string> =
    try
        Directory.GetDirectories(Path.Combine(rootDir, "resource"))
        |> Array.collect (fun subDir ->
            Directory.GetFiles subDir
            |> Array.map (fun file -> Path.GetRelativePath(rootDir, file)))
        |> Ok
    with ex ->
        Error ex.Message

let listFile () : HttpHandler =
    fun next ctx ->
        task {
            let handler =
                (match ctx.TryGetQueryStringValue "dir" with
                 | Some dir -> safeGetFiles config.ContentRoot dir
                 | None -> safeGetAllFiles config.ContentRoot)
                |> function
                    | Ok files -> Successful.ok (responseFactory 200 "獲取fsfs檔案成功" files)
                    | Error msg -> ServerErrors.internalError (responseFactory 500 msg null)

            return! handler next ctx
        }
