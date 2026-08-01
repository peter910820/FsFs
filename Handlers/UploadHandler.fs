module FsFs.Handlers.UploadHandler

open System.IO
open Giraffe
open Microsoft.AspNetCore.Http

open FsFs.Infrastructure.Config
open FsFs.Infrastructure.ResponseFactory

type UploadError =
    | InvalidPath
    | BadRequest
    | NoFileUploaded
    | DirectoryNotFound
    | UnknownError of string

/// <summary>拒絕含 / 或 .. 的上傳目錄名</summary>
let validatePath (dirPath: string) : Result<string, UploadError> =
    if dirPath.Contains "/" || dirPath.Contains ".." then
        Error InvalidPath
    else
        Ok dirPath

let private validateForm (ctx: HttpContext) : Result<IFormFile, UploadError> =
    match ctx.Request.HasFormContentType, ctx.Request.Form.Files.Count with
    | false, _ -> Error BadRequest
    | true, 0 -> Error NoFileUploaded
    | true, _ -> Ok ctx.Request.Form.Files.[0]

/// <summary>確認上傳目標目錄存在</summary>
let ensureDirectory (rootDir: string) (dirPath: string) : Result<string, UploadError> =
    let fullPath = Path.Combine(rootDir, dirPath)

    if Directory.Exists fullPath then
        Ok fullPath
    else
        Error DirectoryNotFound

let private saveFile (fullPath: string) (file: IFormFile) =
    task {
        try
            let savePath = Path.Combine(fullPath, file.FileName)
            use stream = new FileStream(savePath, FileMode.Create)
            do! file.CopyToAsync stream
            return Ok file.FileName
        with ex ->
            return Error(UnknownError ex.Message)
    }

let private toHttpResponse =
    function
    | Ok fileName -> responseFactory StatusCodes.Status200OK $"File {fileName} upload success" None
    | Error InvalidPath -> responseFactory StatusCodes.Status500InternalServerError "Invalid path" None
    | Error BadRequest -> responseFactory StatusCodes.Status400BadRequest "Bad request" None
    | Error NoFileUploaded -> responseFactory StatusCodes.Status400BadRequest "No file uploaded" None
    | Error DirectoryNotFound ->
        responseFactory StatusCodes.Status500InternalServerError "Upload directory does not exist" None
    | Error(UnknownError msg) -> responseFactory StatusCodes.Status500InternalServerError msg None

let uploadHandler (dirPath: string) : HttpHandler =
    fun next ctx ->
        task {
            let prepared =
                validatePath dirPath
                |> Result.bind (fun dir ->
                    validateForm ctx
                    |> Result.bind (fun file ->
                        ensureDirectory config.ContentRoot dir
                        |> Result.map (fun fullPath -> fullPath, file)))

            let! result =
                match prepared with
                | Error err -> task { return Error err }
                | Ok(fullPath, file) -> saveFile fullPath file

            return! toHttpResponse result next ctx
        }
