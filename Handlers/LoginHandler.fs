module FsFs.Handlers.LoginHandler

open System
open BCrypt.Net
open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Caching.Memory

open FsFs.Infrastructure.Config
open FsFs.Infrastructure.Database
open FsFs.Models.DtoModel
open FsFs.Infrastructure.ResponseFactory

let loginHandler : HttpHandler =
    fun next ctx ->
        task {
            let! loginData = ctx.BindJsonAsync<Request.LoginRequest>()
            let db = ctx.GetService<AppDbContext>()
            let! userOpt = tryFindUserByUpdateName db loginData.username

            match userOpt with
            | Some user when BCrypt.Verify(loginData.password, user.Password) ->
                // * 設置Session(Cache)
                let sessionId = Guid.NewGuid().ToString "N"
                cache.Set(sessionId, user.Id, DateTime.UtcNow.AddMinutes 30.0) |> ignore
                // * 設置Cookies
                ctx.Response.Cookies.Append(
                    "sid",
                    sessionId,
                    CookieOptions(
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None, // 跨子網域
                        Domain = config.Domain,
                        Path = "/",
                        Expires = Nullable(DateTimeOffset.UtcNow.AddMinutes 30.0)
                    )
                )

                return! responseFactory StatusCodes.Status200OK "登入成功" (Some loginData.username) next ctx
            | _ -> return! responseFactory StatusCodes.Status401Unauthorized "登入失敗" None next ctx
        }
