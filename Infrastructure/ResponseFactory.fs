module FsFs.Infrastructure.ResponseFactory

open Giraffe
open System.Text.Json
open Microsoft.AspNetCore.Http

open FsFs.Models.DtoModel

let jsonCamelCase<'T> (data: 'T) : HttpHandler =
    fun next ctx ->
        task {
            let options =
                JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase)

            let json = JsonSerializer.Serialize(data, options)
            ctx.Response.ContentType <- "application/json; charset=utf-8"
            do! ctx.Response.WriteAsync json
            return Some ctx
        }

let responseFactory<'T when 'T: not struct and 'T: null> (statusCode: int) (msg: string) (data: 'T) : HttpHandler =
    let resp: Response.ApiResponse<'T> =
        { StatusCode = statusCode
          Msg = msg
          Data = Option.ofObj data }

    setStatusCode statusCode >=> jsonCamelCase resp
