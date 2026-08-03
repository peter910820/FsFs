module FsFs.Infrastructure.Middleware

open System
open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Caching.Memory

open FsFs.Infrastructure.Config
open FsFs.Infrastructure.ResponseFactory

let authMiddleware: HttpHandler =
    fun next ctx ->
        task {
            match ctx.Request.Cookies.TryGetValue "sid" with
            | true, sessionId ->
                match cache.TryGetValue<int> sessionId with
                | true, _ -> return! next ctx
                | false, _ -> return! responseFactory StatusCodes.Status401Unauthorized "階段性認證已過期" None next ctx
            | false, _ -> return! responseFactory StatusCodes.Status401Unauthorized "使用者未登入" None next ctx
        }

/// <summary>從 Authorization header 取出 Bearer token</summary>
let tryGetBearerToken (authorizationHeader: string option) : string option =
    match authorizationHeader with
    | Some value when value.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ->
        let token = value.Substring(7).Trim()

        if String.IsNullOrWhiteSpace token then
            None
        else
            Some token
    | _ -> None

/// <summary>token 是否在允許清單內</summary>
let isApiTokenAllowed (allowed: string list) (token: string) : bool =
    List.contains token allowed

let apiTokenMiddleware: HttpHandler =
    fun next ctx ->
        task {
            let header =
                match ctx.Request.Headers.TryGetValue "Authorization" with
                | true, values when values.Count > 0 -> Some(values.[0])
                | _ -> None

            match tryGetBearerToken header with
            | Some token when isApiTokenAllowed config.ApiTokens token -> return! next ctx
            | _ -> return! responseFactory StatusCodes.Status401Unauthorized "API token 無效或未提供" None next ctx
        }
