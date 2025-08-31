module FsFs.Handlers.UploadHandler

open System.IO
open Giraffe
open Microsoft.AspNetCore.Http

open FsFs.Infrastructure.Config
open FsFs.Infrastructure.ResponseFactory

let uploadHandler (dirPath: string) : HttpHandler =
    fun next ctx ->
        task {
            try
                match ctx.Request.HasFormContentType, ctx.Request.Form.Files.Count with
                | false, _ -> return! responseFactory StatusCodes.Status400BadRequest "Bad request" null next ctx
                | true, 0 -> return! responseFactory StatusCodes.Status400BadRequest "No file uploaded" null next ctx
                | true, _ ->
                    let fullPath = Path.Combine(config.ContentRoot, "resource", dirPath)

                    match Directory.Exists fullPath with
                    | false ->
                        return!
                            responseFactory StatusCodes.Status500InternalServerError "Upload directory does not exist" null next ctx
                    | true ->
                        let file = ctx.Request.Form.Files.[0]
                        let savePath = Path.Combine(fullPath, file.FileName)
                        use stream = new FileStream(savePath, FileMode.Create)
                        do! file.CopyToAsync stream

                        return!
                            responseFactory StatusCodes.Status200OK $"File {file.FileName} upload success" null next ctx
            with _ as ex ->
                return! responseFactory StatusCodes.Status500InternalServerError ex.Message null next ctx
        }
