module FsFs.Handlers.CreateDirectoryHandler

open System.IO
open Giraffe
open Microsoft.AspNetCore.Http

open FsFs.Infrastructure.Config
open FsFs.Infrastructure.ResponseFactory

let createDirectoryHandler (dirName: string) : HttpHandler =
    fun next ctx ->
        task {
            let trimName =
                dirName |> String.filter (fun ch -> not (System.Char.IsWhiteSpace ch))

            if trimName.Contains "/" || trimName.Contains ".." then
                return! responseFactory StatusCodes.Status500InternalServerError "Invalid path" null next ctx
            else
                let fullPath = Path.Combine(config.ContentRoot, "resource", trimName)

                let handler =
                    match trimName, Directory.Exists fullPath with
                    | "", _ -> responseFactory StatusCodes.Status400BadRequest "建立資料夾不得為空" null
                    | _, true -> responseFactory StatusCodes.Status200OK "資料夾已存在，此次請求不做任何操作" null
                    | _, false ->
                        Directory.CreateDirectory fullPath |> ignore
                        responseFactory StatusCodes.Status200OK "資料夾已建立完成" null

                return! handler next ctx
        }
