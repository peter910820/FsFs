module SeaottermsSiteFileserver.Handlers.ErrorHandler

open System
open Microsoft.Extensions.Logging
open Giraffe

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message