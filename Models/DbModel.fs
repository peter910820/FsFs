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

      [<Column("name")>]
      [<Required>]
      Name: string

      [<Column("discord_id")>]
      DiscordId: string

      [<Column("avatar")>]
      Avatar: string

      [<Column("description")>]
      Description: string

      [<Column("private_game_data")>]
      PrivateGameData: bool

      [<Column("role")>]
      Role: int

      [<Column("created_at")>]
      CreatedAt: DateTime

      [<Column("updated_at")>]
      UpdatedAt: DateTime }

[<CLIMutable>]
[<Table("user_auths")>]
type UserAuth =
    { [<Column("user_id")>]
      [<Key>]
      [<DatabaseGenerated(DatabaseGeneratedOption.None)>]
      UserId: int

      [<Column("username")>]
      [<Required>]
      [<MaxLength(30)>]
      Username: string

      [<Column("password")>]
      [<Required>]
      Password: string

      [<Column("created_at")>]
      CreatedAt: DateTime

      [<Column("updated_at")>]
      UpdatedAt: DateTime }
