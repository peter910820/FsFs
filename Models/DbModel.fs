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

      [<Column("management")>]
      Management: bool

      [<Column("created_at")>]
      CreatedAt: DateTime

      [<Column("create_name")>]
      [<Required>]
      CreateName: string

      [<Column("updated_at")>]
      UpdatedAt: DateTime

      [<Column("update_name")>]
      [<Required>]
      UpdateName: string }
