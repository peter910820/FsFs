module SeaottermsSiteFileserver.Handlers.UploadHandler

open System.IO
open Giraffe

open SeaottermsSiteFileserver.Infrastructure.Config
open SeaottermsSiteFileserver.Infrastructure.ResponseFactory

let uploadHandler (dirPath: string) : HttpHandler =
    fun next ctx ->
        task {
            try
                match ctx.Request.HasFormContentType, ctx.Request.Form.Files.Count with
                | false, _ -> return! RequestErrors.badRequest (responseFactory 400 "Bad request" null) next ctx
                | true, 0 -> return! RequestErrors.badRequest (responseFactory 400 "No file uploaded" null) next ctx
                | true, _ ->
                    let fullPath = Path.Combine(config.ContentRoot, "resource", dirPath)

                    match Directory.Exists fullPath with
                    | false ->
                        return!
                            ServerErrors.internalError
                                (responseFactory 500 "Upload directory does not exist" null)
                                next
                                ctx
                    | true ->
                        let file = ctx.Request.Form.Files.[0]
                        let savePath = Path.Combine(fullPath, file.FileName)
                        use stream = new FileStream(savePath, FileMode.Create)
                        do! file.CopyToAsync stream
                        return! Successful.ok (responseFactory 200 $"File {file.FileName} upload success" null) next ctx
            with _ as ex ->
                return! ServerErrors.internalError (responseFactory 500 ex.Message null) next ctx
        }
