module SeaottermsSiteFileserver.Handlers.LoginHandler

open BCrypt.Net
open System.Linq
open Giraffe
open Microsoft.EntityFrameworkCore

open SeaottermsSiteFileserver.DbConnect
open SeaottermsSiteFileserver.Models.DtoModel

let loginHandler () : HttpHandler =
     fun next ctx ->
        task {
            let! loginData = ctx.BindJsonAsync<LoginRequest>()
            let db = ctx.GetService<AppDbContext>()
            let! user =
                db.Users
                    .Where(fun u -> u.UpdateName = loginData.username)
                    .FirstOrDefaultAsync()
                |> Async.AwaitTask
            
            match Option.ofObj user with
                | Some user when BCrypt.Verify(loginData.password, user.Password) ->
                    return! setStatusCode 200 next ctx
                | _ ->
                    return! setStatusCode 401 next ctx
        }