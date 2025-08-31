module FsFs.Infrastructure.Middleware

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
                | false, _ -> return! responseFactory StatusCodes.Status401Unauthorized "階段性認證已過期" null next ctx
            | false, _ -> return! responseFactory StatusCodes.Status401Unauthorized "使用者未登入" null next ctx
        }
