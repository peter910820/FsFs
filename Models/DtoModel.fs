module FsFs.Models.DtoModel

open System

/// <summary>
/// Request DTO的結構定義
/// </summary>
module Request =
    type LoginRequest = { username: string; password: string }

/// <summary>
/// Response DTO的結構定義
/// </summary>
module Response =
    type ApiResponse<'T> =
        { StatusCode: int
          Data: 'T
          Msg: string }

    type LoginResponse =
        { Token: string
          Username: string
          Email: string
          Avatar: string
          Exp: int
          Management: bool
          CreatedAt: DateTime }
