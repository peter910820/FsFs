module FsFs.Tests.DtoModelTests

open System.Text.Json
open Xunit
open FsFs.Models.DtoModel

[<Fact>]
let ``ApiResponse serializes with camelCase property names`` () =
    let resp: Response.ApiResponse<string> =
        { StatusCode = 200
          Msg = "ok"
          Data = Some "hello" }

    let options =
        JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase)

    let json = JsonSerializer.Serialize(resp, options)

    Assert.Contains(""""statusCode":200""", json)
    Assert.Contains(""""msg":"ok""", json)
    Assert.Contains(""""data":"hello""", json)

[<Fact>]
let ``ApiResponse with None data serializes data as null`` () =
    let resp: Response.ApiResponse<string> =
        { StatusCode = 500
          Msg = "error"
          Data = None }

    let options =
        JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase)

    let json = JsonSerializer.Serialize(resp, options)

    Assert.Contains(""""data":null""", json)

[<Fact>]
let ``LoginResponse serializes with camelCase including isAdmin`` () =
    let resp: Response.LoginResponse =
        { Username = "seaotterms"
          Email = "p@example.com"
          Avatar = ""
          Exp = 1
          IsAdmin = true
          CreatedAt = System.DateTime(2024, 1, 1, 0, 0, 0, System.DateTimeKind.Utc) }

    let options =
        JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase)

    let json = JsonSerializer.Serialize(resp, options)

    Assert.Contains(""""username":"seaotterms""", json)
    Assert.Contains(""""isAdmin":true""", json)
    Assert.Contains(""""exp":1""", json)

[<Fact>]
let ``DeleteFileRequest holds fileName`` () =
    let req: Request.DeleteFileRequest = { fileName = "resource/a.txt" }
    Assert.Equal("resource/a.txt", req.fileName)

[<Fact>]
let ``LoginRequest holds username and password`` () =
    let req: Request.LoginRequest =
        { username = "seaotterms"
          password = "secret" }

    Assert.Equal("seaotterms", req.username)
    Assert.Equal("secret", req.password)
