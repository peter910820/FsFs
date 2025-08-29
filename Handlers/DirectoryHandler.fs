module SeaottermsSiteFileserver.Handlers.DirectoryHandler

open System.IO
open Giraffe

open SeaottermsSiteFileserver.Infrastructure.Config

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
                    | Ok folders -> setStatusCode 200 >=> json folders
                    | Error msg -> setStatusCode 500 >=> json {| error = msg |}

            return! handler next ctx
        }
