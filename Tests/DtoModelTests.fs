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
let ``ApiResponse with null data serializes data as null`` () =
    let resp: Response.ApiResponse<string> =
        { StatusCode = 500
          Msg = "error"
          Data = None }

    let options =
        JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase)

    let json = JsonSerializer.Serialize(resp, options)

    Assert.Contains(""""data":null""", json)

[<Fact>]
let ``LoginRequest holds username and password`` () =
    let req: Request.LoginRequest =
        { username = "peter"
          password = "secret" }

    Assert.Equal("peter", req.username)
    Assert.Equal("secret", req.password)
