module SeaottermsSiteFileserver.DbConnect

open Microsoft.EntityFrameworkCore
open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.EntityFrameworkCore
open System.Linq


open SeaottermsSiteFileserver.Models.DbModel

type AppDbContext(options: DbContextOptions<AppDbContext>) =
    inherit DbContext(options)

    [<DefaultValue>]
    val mutable users : DbSet<User>
    member this.Users with get() = this.users and set v = this.users <- v

let checkDbConnection (services: IServiceProvider) : Async<Result<unit,string>> =
    async {
        try
            use scope = services.CreateScope()
            let db = scope.ServiceProvider.GetRequiredService<AppDbContext>()
            let! _ = db.Users.Take(1).ToListAsync() |> Async.AwaitTask
            return Ok ()
        with ex ->
            return Error ex.Message
    }