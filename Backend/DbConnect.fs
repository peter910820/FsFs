module SeaottermsSiteFileserver.DbConnect

open Microsoft.EntityFrameworkCore

open SeaottermsSiteFileserver.Models.DbModel

type AppDbContext(options: DbContextOptions<AppDbContext>) =
    inherit DbContext(options)

    member val Users : DbSet<User> = null with get, set
