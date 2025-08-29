module SeaottermsSiteFileserver.Handlers.DirectoryHandler

open System.IO
open Giraffe

open SeaottermsSiteFileserver.Infrastructure.Config
open SeaottermsSiteFileserver.Infrastructure.ResponseFactory

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
                    | Ok folders -> Successful.ok (responseFactory 200 "獲取fsfs資料夾成功" folders)
                    | Error msg -> ServerErrors.internalError (responseFactory 500 msg null)

            return! handler next ctx
        }
