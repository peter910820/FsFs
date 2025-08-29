module FsFs.Infrastructure.ResponseFactory

open Giraffe

open FsFs.Models.DtoModel


let responseFactory (statusCode: int) (msg: string) (data: 'T) : HttpHandler =
    let resp: ApiResponse<'T> =
        { StatusCode = statusCode
          Msg = msg
          Data = data }

    setStatusCode statusCode >=> json resp
