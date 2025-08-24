module SeaottermsSiteFileserver.Routers.ApiRouter

open Giraffe

open SeaottermsSiteFileserver.Handlers.DirectoryHandler
open SeaottermsSiteFileserver.Handlers.FileHandler

let apiRoutes : HttpHandler =
    choose [
        GET >=>
            choose [
                route "/directory" >=> listFolders ()
                route "/files" >=> listFile ()
            ]
        POST >=> route "/upload" >=> Successful.NO_CONTENT
    ]