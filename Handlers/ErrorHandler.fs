module SeaottermsSiteFileserver.Handlers.ErrorHandler

open System
open Microsoft.Extensions.Logging
open Giraffe

open SeaottermsSiteFileserver.Infrastructure.ResponseFactory

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, ex.Message)

    clearResponse
    >=> ServerErrors.internalError (responseFactory 500 "發生內部錯誤，請聯繫管理員" null)
