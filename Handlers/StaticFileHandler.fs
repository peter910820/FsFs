module FsFs.Handlers.StaticFileHandler

open System.IO
open Giraffe
open Microsoft.AspNetCore.Http

open FsFs.Infrastructure.ResponseFactory

let serveStaticFile (folderPath: string) (fileName: string) : HttpHandler =
    fun next ctx ->
        task {
            let filePath = Path.Combine(folderPath, fileName)

            match filePath with
            | path when File.Exists path ->
                ctx.Response.ContentType <-
                    match Path.GetExtension(filePath).ToLower() with
                    | ".js" -> "text/javascript;"
                    | ".mjs" -> "text/javascript;"
                    | ".cjs" -> "text/javascript;"
                    | ".css" -> "text/css;"
                    | ".html" -> "text/html;"
                    | ".png" -> "image/png"
                    | ".jpg"
                    | ".jpeg" -> "image/jpeg"
                    | ".gif" -> "image/gif"
                    | ".svg" -> "image/svg+xml"
                    | _ -> "application/octet-stream"

                do! ctx.Response.SendFileAsync filePath
                return Some ctx
            | _ -> return! RequestErrors.notFound (responseFactory 404 "檔案不存在" null) next ctx
        }
