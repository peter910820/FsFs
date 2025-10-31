module FsFs.Models.DtoModel

open System

/// <summary>Request DTO的結構定義</summary>
module Request =
    type LoginRequest = { username: string; password: string }

    type DeleteFileRequest = { fileName: string }

/// <summary>Response DTO的結構定義</summary>
module Response =
    type ApiResponse<'T> =
        { StatusCode: int
          Msg: string
          Data: 'T option }

    type LoginResponse =
        { Token: string
          Username: string
          Email: string
          Avatar: string
          Exp: int
          Management: bool
          CreatedAt: DateTime }
