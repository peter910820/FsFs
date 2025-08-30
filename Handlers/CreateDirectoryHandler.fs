module FsFs.Handlers.CreateDirectoryHandler

open Giraffe

let createDirectoryHandler () : HttpHandler =
    fun next ctx ->
        task {
                return! next ctx
            }
