module SeaottermsSiteFileserver.Handlers.UploadHandler

open System.IO
open Giraffe

open SeaottermsSiteFileserver.Infrastructure.Config

let uploadHandler (dirPath: string) : HttpHandler =
    fun next ctx ->
        task {
            try
                match ctx.Request.HasFormContentType, ctx.Request.Form.Files.Count with
                | false, _ -> return! RequestErrors.BAD_REQUEST "Bad request" next ctx
                | true, 0 -> return! RequestErrors.BAD_REQUEST "No file uploaded" next ctx
                | true, _ ->
                    let fullPath = Path.Combine(config.ContentRoot, "resource", dirPath)
                    printfn "%s" fullPath

                    match Directory.Exists fullPath with
                    | false -> return! RequestErrors.BAD_REQUEST "Upload directory does not exist" next ctx
                    | true ->
                        let file = ctx.Request.Form.Files.[0]
                        let savePath = Path.Combine(fullPath, file.FileName)
                        use stream = new FileStream(savePath, FileMode.Create)
                        do! file.CopyToAsync stream
                        return! text "Files uploaded!" next ctx
            with _ as ex ->
                return! ServerErrors.INTERNAL_ERROR ("Upload failed: " + ex.Message) next ctx
        }
