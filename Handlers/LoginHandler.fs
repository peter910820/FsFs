module SeaottermsSiteFileserver.Handlers.LoginHandler

open BCrypt.Net
open System.Linq
open Giraffe
open Microsoft.EntityFrameworkCore

open SeaottermsSiteFileserver.Infrastructure.Database
open SeaottermsSiteFileserver.Models.DtoModel
open SeaottermsSiteFileserver.Infrastructure.ResponseFactory

let loginHandler () : HttpHandler =
    fun next ctx ->
        task {
            let! loginData = ctx.BindJsonAsync<LoginRequest>()
            let db = ctx.GetService<AppDbContext>()

            let! user =
                db.Users.Where(fun u -> u.UpdateName = loginData.username).FirstOrDefaultAsync()
                |> Async.AwaitTask

            match Option.ofObj user with
            | Some user when BCrypt.Verify(loginData.password, user.Password) ->
                return! Successful.ok (responseFactory 200 "登入成功" loginData.username) next ctx
            | _ -> return! (setStatusCode 401 >=> responseFactory 401 "登入失敗" null) next ctx
        }
