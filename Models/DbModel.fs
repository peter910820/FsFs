module FsFs.Models.DbModel

open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

// ---------------------------------
// DB Models
// ---------------------------------

[<CLIMutable>]
[<Table("users")>]
type User =
    { [<Column("id")>]
      Id: int

      [<Column("username")>]
      [<Required>]
      Username: string

      [<Column("password")>]
      [<Required>]
      Password: string

      [<Column("email")>]
      [<Required>]
      Email: string

      [<Column("avatar")>]
      Avatar: string

      [<Column("exp")>]
      Exp: int

      [<Column("is_admin")>]
      IsAdmin: bool

      [<Column("created_at")>]
      CreatedAt: DateTime

      [<Column("updated_at")>]
      UpdatedAt: DateTime

      [<Column("deleted_at")>]
      DeletedAt: DateTime Nullable }
