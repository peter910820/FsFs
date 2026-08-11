module FsFs.Infrastructure.Database

open System
open System.Linq
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.DependencyInjection

open FsFs.Models.DbModel

type AppDbContext(options: DbContextOptions<AppDbContext>) =
    inherit DbContext(options)

    [<DefaultValue>]
    val mutable private users: DbSet<User>

    [<DefaultValue>]
    val mutable private userAuths: DbSet<UserAuth>

    member this.Users
        with get () = this.users
        and set v = this.users <- v

    member this.UserAuths
        with get () = this.userAuths
        and set v = this.userAuths <- v

let tryFindAuthByUsername (db: AppDbContext) (username: string) =
    task {
        let! auth = db.UserAuths.AsNoTracking().FirstOrDefaultAsync(fun a -> a.Username = username)
        return Option.ofObj auth
    }

let tryFindUserById (db: AppDbContext) (userId: int) =
    task {
        let! user = db.Users.AsNoTracking().FirstOrDefaultAsync(fun u -> u.Id = userId)
        return Option.ofObj user
    }

let checkDbConnection (services: IServiceProvider) : Async<Result<unit, string>> =
    async {
        try
            use scope = services.CreateScope()
            let db = scope.ServiceProvider.GetRequiredService<AppDbContext>()
            let! _ = db.Users.OrderBy(fun u -> u.Id).Take(1).ToListAsync() |> Async.AwaitTask
            return Ok()
        with ex ->
            return Error ex.Message
    }
