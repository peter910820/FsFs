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


/// <summary>列出指定子目錄檔案；拒絕 path traversal</summary>
let safeGetFiles (rootDir: string) (subPath: string) : Result<string[], string> =
    if subPath.Contains "/" || subPath.Contains ".." then
        Error "Invalid path"
    else
        try
            Directory.GetFiles(Path.Combine(rootDir, subPath))
            |> Array.map (fun file -> Path.GetRelativePath(rootDir, file))
            |> Ok
        with ex ->
            Error ex.Message

/// <summary>列出根目錄下各子資料夾內的檔案</summary>
let safeGetAllFiles (rootDir: string) : Result<string[], string> =
    try
        Directory.GetDirectories rootDir
        |> Array.collect (fun subDir ->
            Directory.GetFiles subDir
            |> Array.map (fun file -> Path.GetRelativePath(rootDir, file)))
        |> Ok
    with ex ->
        Error ex.Message

/// <summary>依 CreationTimeUtc 取最近檔案，最多 10 筆</summary>
let safeGetRecentFiles (rootDir: string) (limit: int) : Result<Response.RecentFileItem[], string> =
    let takeCount = min (max limit 1) 10

    try
        Directory.GetDirectories rootDir
        |> Array.collect (fun subDir ->
            try
                Directory.GetFiles subDir
            with _ ->
                [||])
        |> Array.choose (fun fullPath ->
            try
                let info = FileInfo fullPath

                Some
                    { Response.RecentFileItem.Path = Path.GetRelativePath(rootDir, fullPath)
                      Response.RecentFileItem.CreatedAt = info.CreationTimeUtc }
            with _ ->
                None)
        |> Array.sortByDescending (fun f -> f.CreatedAt)
        |> Array.truncate takeCount
        |> Ok
    with ex ->
        Error ex.Message

let listFile : HttpHandler =
    fun next ctx ->
        task {
            let handler =
                (match ctx.TryGetQueryStringValue "dir" with
                 | Some dir -> safeGetFiles config.ContentRoot dir
                 | None -> safeGetAllFiles config.ContentRoot)
                |> function
                    | Ok files -> responseFactory StatusCodes.Status200OK "獲取fsfs檔案成功" (Some files)
                    | Error msg -> responseFactory StatusCodes.Status500InternalServerError msg None

            return! handler next ctx
        }

let listRecentFiles : HttpHandler =
    fun next ctx ->
        task {
            let limit =
                match ctx.TryGetQueryStringValue "limit" with
                | Some raw ->
                    match System.Int32.TryParse raw with
                    | true, n -> n
                    | _ -> 10
                | None -> 10

            let handler =
                safeGetRecentFiles config.ContentRoot limit
                |> function
                    | Ok files -> responseFactory StatusCodes.Status200OK "獲取最近檔案成功" (Some files)
                    | Error msg -> responseFactory StatusCodes.Status500InternalServerError msg None

            return! handler next ctx
        }

/// <summary>刪除檔案，有副作用</summary>
let safeDeleteFile path : Result<unit, DeleteFileError> =
    if not (File.Exists path) then
        Error (FileNotFound path)
    else
        try
            File.Delete path
            Ok()
        with ex ->
            Error (UnknownError ex.Message)

/// <summary>刪除檔案Handler</summary>
let deleteFileHandler : HttpHandler =
    fun next ctx ->
        task {
            let! req = ctx.BindJsonAsync<Request.DeleteFileRequest>()

            let handler =
                match safeDeleteFile (Path.Combine(config.ContentRoot, req.fileName)) with
                | Ok() -> responseFactory StatusCodes.Status200OK "刪除檔案成功" None
                | Error (FileNotFound msg) -> responseFactory StatusCodes.Status500InternalServerError msg (Some msg)
                | Error (UnknownError msg) -> responseFactory StatusCodes.Status500InternalServerError msg (Some msg)

            return! handler next ctx
        }
