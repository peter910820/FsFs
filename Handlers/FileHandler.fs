module SeaottermsSiteFileserver.Handlers.FileHandler

open System.IO
open Giraffe

open SeaottermsSiteFileserver.Config

let private safeGetFiles (rootDir: string) (subPath: string) : Result<string[], string> =
    try
        Directory.GetFiles (Path.Combine(rootDir, "resource", subPath))
        |> Ok
    with ex -> Error ex.Message

let private safeGetAllFiles  (rootDir: string) : Result<string[], string> =
    try
        Directory.GetDirectories (Path.Combine(rootDir, "resource"))
        |> Array.collect (fun subDir ->
            Directory.GetFiles subDir
            |> Array.map (fun file ->
                Path.GetRelativePath(rootDir, file)
            )
        )
        |> Ok
    with ex -> Error ex.Message

let listFile () : HttpHandler =
    fun next ctx ->
        task {
            let handler = 
                (match ctx.TryGetQueryStringValue "dir" with
                | Some dir -> safeGetFiles rootDir dir
                | None -> safeGetAllFiles rootDir)
                |> function
                    | Ok files -> setStatusCode 200 >=> json files
                    | Error msg  -> setStatusCode 500 >=> json {| error = msg |}

            return! handler next ctx
        }