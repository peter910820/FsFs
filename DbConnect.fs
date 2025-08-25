module SeaottermsSiteFileserver.DbConnect

open Microsoft.EntityFrameworkCore

open SeaottermsSiteFileserver.Models.Model

type AppDbContext(options: DbContextOptions<AppDbContext>) =
    inherit DbContext(options)

    member val Users : DbSet<User> = null with get, set
