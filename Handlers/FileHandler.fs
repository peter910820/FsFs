module FsFs.Handlers.FileHandler

open System.IO
open Giraffe
open Microsoft.AspNetCore.Http

open FsFs.Infrastructure.Config
open FsFs.Infrastructure.ResponseFactory
open FsFs.Models.DtoModel

type DeleteFileError =
    | FileNotFound of string
    | UnknownError of string


let private safeGetFiles (rootDir: string) (subPath: string) : Result<string[], string> =
    if subPath.Contains "/" || subPath.Contains ".." then
        Error "Invalid path"
    else
        try
            Directory.GetFiles(Path.Combine(rootDir, subPath))
            |> Array.map (fun file -> Path.GetRelativePath(rootDir, file))
            |> Ok
        with ex ->
            Error ex.Message

let private safeGetAllFiles (rootDir: string) : Result<string[], string> =
    try
        Directory.GetDirectories(Path.Combine rootDir)
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
                    | Ok files -> responseFactory StatusCodes.Status200OK "獲取fsfs檔案成功" files
                    | Error msg -> responseFactory StatusCodes.Status500InternalServerError msg null

            return! handler next ctx
        }

/// <summary>刪除檔案，有副作用</summary>
let safeDeleteFile path : Result<unit, DeleteFileError> =
    if not (File.Exists path) then
        Error(FileNotFound path)
    else
        try
            File.Delete path
            Ok()
        with ex ->
            Error(UnknownError ex.Message)

/// <summary>刪除檔案Handler</summary>
let deleteFileHandler () : HttpHandler =
    fun next ctx ->
        task {
            let! req = ctx.BindJsonAsync<Request.DeleteFileRequest>()

            let handler =
                match safeDeleteFile (Path.Combine(config.ContentRoot, req.fileName)) with
                | Ok() -> responseFactory StatusCodes.Status200OK "刪除檔案成功" null
                | Error(FileNotFound msg) -> responseFactory StatusCodes.Status500InternalServerError msg msg
                | Error(UnknownError msg) -> responseFactory StatusCodes.Status500InternalServerError msg msg

            return! handler next ctx
        }
