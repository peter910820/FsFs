module FsFs.Handlers.ErrorHandler

open System
open Microsoft.Extensions.Logging
open Giraffe
open Microsoft.AspNetCore.Http

open FsFs.Infrastructure.ResponseFactory

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, ex.Message)

    clearResponse
    >=> responseFactory StatusCodes.Status500InternalServerError "發生內部錯誤，請聯繫管理員" null
