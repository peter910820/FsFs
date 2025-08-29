module SeaottermsSiteFileserver.Routers.ResourceRouter

open System.IO
open Giraffe

open SeaottermsSiteFileserver.Infrastructure.Config
open SeaottermsSiteFileserver.Handlers.StaticFileHandler

let resourceDir = Path.Combine(config.ContentRoot, "resource")

let staticFileRoutes: HttpHandler =
    choose
        [ GET
          >=> choose
                  [ routef "/code/%s" (serveStaticFile (Path.Combine(resourceDir, "code")))
                    routef "/image/%s" (serveStaticFile (Path.Combine(resourceDir, "image")))
                    routef "/technology/%s" (serveStaticFile (Path.Combine(resourceDir, "technology")))
                    routef "/test/%s" (serveStaticFile (Path.Combine(resourceDir, "test")))
                    routef "/test2/%s" (serveStaticFile (Path.Combine(resourceDir, "test2"))) ] ]
