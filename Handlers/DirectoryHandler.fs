module FsFs.Handlers.DirectoryHandler

open System.IO
open Giraffe
open Microsoft.AspNetCore.Http

open FsFs.Infrastructure.Config
open FsFs.Infrastructure.ResponseFactory

let private safeGetDirectories (rootDir: string) : Result<string[], string> =
    try
        Directory.GetDirectories(Path.Combine(rootDir, "resource"))
        |> Array.map (fun d -> DirectoryInfo(d).Name)
        |> Ok
    with ex ->
        Error ex.Message

let listFolders () : HttpHandler =
    fun next ctx ->
        task {
            let handler =
                safeGetDirectories config.ContentRoot
                |> function
                    | Ok folders -> responseFactory StatusCodes.Status200OK "獲取fsfs資料夾成功" folders
                    | Error msg -> responseFactory StatusCodes.Status500InternalServerError msg null

            return! handler next ctx
        }
