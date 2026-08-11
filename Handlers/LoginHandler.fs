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

let private minLoginRole = 5

let loginHandler: HttpHandler =
    fun next ctx ->
        task {
            let! loginData = ctx.BindJsonAsync<Request.LoginRequest>()
            let db = ctx.GetService<AppDbContext>()
            let! authOpt = tryFindAuthByUsername db loginData.username

            match authOpt with
            | Some auth when BCrypt.Verify(loginData.password, auth.Password) ->
                let! userOpt = tryFindUserById db auth.UserId

                match userOpt with
                | Some user when user.Role >= minLoginRole ->
                    // * 設置Session(Cache)
                    let sessionId = Guid.NewGuid().ToString "N"
                    cache.Set(sessionId, user.Id, DateTime.UtcNow.AddMinutes 30.0) |> ignore
                    // * 設置Cookies
                    let cookieOptions =
                        CookieOptions(
                            HttpOnly = true,
                            Secure = config.CookieSecure,
                            SameSite = config.CookieSameSite,
                            Path = "/",
                            Expires = Nullable(DateTimeOffset.UtcNow.AddMinutes 30.0)
                        )

                    config.Domain
                    |> Option.iter (fun domain -> cookieOptions.Domain <- domain)

                    ctx.Response.Cookies.Append("sid", sessionId, cookieOptions)

                    let loginResponse: Response.LoginResponse =
                        { Username = auth.Username
                          Name = user.Name
                          Avatar = user.Avatar
                          CreatedAt = user.CreatedAt }

                    return! responseFactory StatusCodes.Status200OK "登入成功" (Some loginResponse) next ctx
                | Some _ ->
                    return!
                        responseFactory StatusCodes.Status403Forbidden "權限不足，無法登入" None next ctx
                | None -> return! responseFactory StatusCodes.Status401Unauthorized "登入失敗" None next ctx
            | _ -> return! responseFactory StatusCodes.Status401Unauthorized "登入失敗" None next ctx
        }
