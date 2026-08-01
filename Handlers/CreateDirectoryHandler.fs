module FsFs.Handlers.CreateDirectoryHandler

open System.IO
open Giraffe
open Microsoft.AspNetCore.Http

open FsFs.Infrastructure.Config
open FsFs.Infrastructure.ResponseFactory

type CreateDirectoryError =
    | InvalidPath
    | EmptyName

type CreateDirectoryOutcome =
    | Created
    | AlreadyExists

let normalizeDirName (dirName: string) =
    dirName |> String.filter (fun ch -> not (System.Char.IsWhiteSpace ch))

let validateDirName (trimName: string) : Result<string, CreateDirectoryError> =
    if trimName.Contains "/" || trimName.Contains ".." then
        Error InvalidPath
    elif trimName = "" then
        Error EmptyName
    else
        Ok trimName

/// <summary>在 root 下建立資料夾；已存在則視為成功且無副作用</summary>
let tryCreateDirectory (rootDir: string) (trimName: string) : CreateDirectoryOutcome =
    let fullPath = Path.Combine(rootDir, trimName)

    if Directory.Exists fullPath then
        AlreadyExists
    else
        Directory.CreateDirectory fullPath |> ignore
        Created

let createDirectoryHandler (dirName: string) : HttpHandler =
    fun next ctx ->
        task {
            let trimName = normalizeDirName dirName

            match validateDirName trimName with
            | Error InvalidPath ->
                return! responseFactory StatusCodes.Status500InternalServerError "Invalid path" None next ctx
            | Error EmptyName ->
                return! responseFactory StatusCodes.Status400BadRequest "建立資料夾不得為空" None next ctx
            | Ok name ->
                let msg =
                    match tryCreateDirectory config.ContentRoot name with
                    | AlreadyExists -> "資料夾已存在，此次請求不做任何操作"
                    | Created -> "資料夾已建立完成"

                return! responseFactory StatusCodes.Status200OK msg None next ctx
        }
