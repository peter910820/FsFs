module FsFs.Models.DbModel

open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

// ---------------------------------
// DB Models
// ---------------------------------

open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

[<Table("users")>]
type User() =
    [<Column("id")>]
    member val Id: int = 0 with get, set

    [<Column("username")>]
    [<Required>]
    member val Username: string = "" with get, set

    [<Column("password")>]
    [<Required>]
    member val Password: string = "" with get, set

    [<Column("email")>]
    [<Required>]
    member val Email: string = "" with get, set

    [<Column("avatar")>]
    member val Avatar: string = "" with get, set

    [<Column("exp")>]
    member val Exp: int = 0 with get, set

    [<Column("management")>]
    member val Management: bool = false with get, set

    [<Column("created_at")>]
    member val CreatedAt: DateTime = DateTime.MinValue with get, set

    [<Column("create_name")>]
    [<Required>]
    member val CreateName: string = "" with get, set

    [<Column("updated_at")>]
    member val UpdatedAt: DateTime = DateTime.MinValue with get, set

    [<Column("update_name")>]
    member val UpdateName: string = "" with get, set