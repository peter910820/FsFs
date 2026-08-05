module FsFs.Handlers.ServerUploadHandler

open System
open System.IO
open Giraffe
open Microsoft.AspNetCore.Http

open FsFs.Handlers.UploadHandler
open FsFs.Infrastructure.Config
open FsFs.Infrastructure.ResponseFactory
open FsFs.Models.DtoModel

type ServerUploadError =
    | InvalidPath
    | InvalidFileName
    | BlockedExtension
    | InvalidBase64
    | DirectoryNotFound
    | UnknownError of string

let private blockedExtensions = [ ".js"; ".exe"; ".dll"; ".sh" ]

/// <summary>驗證上傳檔名（拒絕路徑字元、空白、禁止副檔名）</summary>
let validateFileName (fileName: string) : Result<string, ServerUploadError> =
    if String.IsNullOrWhiteSpace fileName then
        Error InvalidFileName
    elif
        fileName.Contains "/"
        || fileName.Contains "\\"
        || fileName.Contains ".."
        || fileName <> Path.GetFileName fileName
    then
        Error InvalidFileName
    elif blockedExtensions |> List.exists (fun ext -> fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) then
        Error BlockedExtension
    else
        Ok fileName

/// <summary>解碼 Base64 內容</summary>
let decodeBase64 (contentBase64: string) : Result<byte[], ServerUploadError> =
    if String.IsNullOrWhiteSpace contentBase64 then
        Error InvalidBase64
    else
        try
            Ok(Convert.FromBase64String contentBase64)
        with _ ->
            Error InvalidBase64

/// <summary>將位元組寫入目標目錄</summary>
let writeUploadBytes (rootDir: string) (dirPath: string) (fileName: string) (bytes: byte[]) : Result<Response.ServerUploadResult, ServerUploadError> =
    try
        let savePath = Path.Combine(rootDir, dirPath, fileName)
        File.WriteAllBytes(savePath, bytes)
        let info = FileInfo savePath

        Ok
            { Response.ServerUploadResult.Path = Path.GetRelativePath(rootDir, savePath)
              Response.ServerUploadResult.CreatedAt = info.CreationTimeUtc }
    with ex ->
        Error (UnknownError ex.Message)

let private mapUploadPathError =
    function
    | UploadError.InvalidPath -> InvalidPath
    | UploadError.DirectoryNotFound -> DirectoryNotFound
    | other -> UnknownError(sprintf "%A" other)

let private toHttpResponse =
    function
    | Ok(result: Response.ServerUploadResult) ->
        responseFactory StatusCodes.Status200OK $"File {Path.GetFileName result.Path} upload success" (Some result)
    | Error InvalidPath -> responseFactory StatusCodes.Status400BadRequest "Invalid path" None
    | Error InvalidFileName -> responseFactory StatusCodes.Status400BadRequest "Invalid file name" None
    | Error BlockedExtension -> responseFactory StatusCodes.Status400BadRequest "Blocked file extension" None
    | Error InvalidBase64 -> responseFactory StatusCodes.Status400BadRequest "Invalid base64 content" None
    | Error DirectoryNotFound ->
        responseFactory StatusCodes.Status500InternalServerError "Upload directory does not exist" None
    | Error (UnknownError msg) -> responseFactory StatusCodes.Status500InternalServerError msg None

let serverUploadHandler (dirPath: string) : HttpHandler =
    fun next ctx ->
        task {
            let! body = ctx.BindJsonAsync<Request.ServerUploadRequest>()

            let result =
                validatePath dirPath
                |> Result.mapError mapUploadPathError
                |> Result.bind (fun dir ->
                    validateFileName body.fileName
                    |> Result.bind (fun fileName ->
                        decodeBase64 body.contentBase64
                        |> Result.bind (fun bytes ->
                            ensureDirectory config.ContentRoot dir
                            |> Result.mapError mapUploadPathError
                            |> Result.bind (fun _ -> writeUploadBytes config.ContentRoot dir fileName bytes))))

            return! toHttpResponse result next ctx
        }
