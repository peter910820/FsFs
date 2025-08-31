module FsFs.Handlers.AuthHandler

open Giraffe
open Microsoft.AspNetCore.Http

open FsFs.Infrastructure.ResponseFactory

let authHandler () : HttpHandler =
    fun next ctx -> task { return! Successful.ok (responseFactory StatusCodes.Status200OK "登入驗證成功" null) next ctx }
