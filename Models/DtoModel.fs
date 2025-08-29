module FsFs.Models.DtoModel

// ---------------------------------
// Response Models(unuse)
// ---------------------------------

type ApiResponse<'T> =
    { StatusCode: int
      Data: 'T
      Msg: string }

type LoginRequest = { username: string; password: string }
