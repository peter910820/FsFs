module SeaottermsSiteFileserver.Handlers.ErrorHandler

open System
open Microsoft.Extensions.Logging
open Giraffe

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, ex.Message)
    clearResponse >=> setStatusCode 500 >=> text "發生內部錯誤，請聯繫管理員"