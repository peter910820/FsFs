module SeaottermsSiteFileserver.Models.DbModel

open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

// ---------------------------------
// DB Models
// ---------------------------------

type User() =
    [<Key>]
    [<Column("id")>]
    [<DefaultValue>] val mutable Id : uint32

    [<Column("username")>]
    [<Required>]
    [<DefaultValue>] val mutable Username : string

    [<Column("password")>]
    [<Required>]
    [<DefaultValue>] val mutable Password : string

    [<Column("email")>]
    [<Required>]
    [<DefaultValue>] val mutable Email : string

    [<Column("avatar")>]
    [<DefaultValue>] val mutable Avatar : string

    [<Column("exp")>]
    [<DefaultValue>] val mutable Exp : int

    [<Column("management")>]
    [<DefaultValue>] val mutable Management : bool

    [<Column("created_at")>]
    [<DefaultValue>] val mutable CreatedAt : DateTime

    [<Column("create_name")>]
    [<Required>]
    [<DefaultValue>] val mutable CreateName : string

    [<Column("updated_at")>]
    [<DefaultValue>] val mutable UpdatedAt : DateTime

    [<Column("update_name")>]
    [<DefaultValue>] val mutable UpdateName : string