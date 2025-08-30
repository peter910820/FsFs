module FsFs.Handlers.AuthHandler

open System
open Giraffe
open Microsoft.Extensions.Caching.Memory

open FsFs.Infrastructure.Config
open FsFs.Infrastructure.ResponseFactory

let authHandler () : HttpHandler =
    fun next ctx ->
        task {
            match ctx.Request.Cookies.TryGetValue "sid" with
            | true, sessionId ->
                match cache.TryGetValue<int> sessionId with
                | true, _ -> return! Successful.ok (responseFactory 200 "登入驗證成功" null) next ctx
                | false, _ -> return! (setStatusCode 401 >=> responseFactory 401 "階段性認證已過期" null) next ctx
            | false, _ -> return! (setStatusCode 500 >=> responseFactory 401 "使用者未登入" null) next ctx
        }
