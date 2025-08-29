module SeaottermsSiteFileserver.Models.DtoModel

// ---------------------------------
// Response Models(unuse)
// ---------------------------------

type Message = { Text: string }

type ApiResponse<'T> =
    { StatusCode: int
      Data: 'T option
      ErrorMsg: string option }

type LoginRequest = { username: string; password: string }
