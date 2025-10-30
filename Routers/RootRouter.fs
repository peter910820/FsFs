module FsFs.Routers.RootRouter


open Giraffe

open FsFs.Infrastructure.Config
open FsFs.Routers.ApiRouter
open ResourceRouter
open FsFs.Infrastructure.ResponseFactory

let webApp: HttpHandler =
    choose
        [ if config.StartMode = "Manual" then subRoute "/resource" staticFileRoutes
          subRoute "/api" apiRoutes
          RequestErrors.notFound (responseFactory 404 "not found" null) ]