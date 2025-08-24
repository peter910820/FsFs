module SeaottermsSiteFileserver.Routers.ApiRouter

open Giraffe

open SeaottermsSiteFileserver.Handlers.DirectoryHanler

let apiRoutes : HttpHandler =
    choose [
        GET >=>
            choose [
                route "/directory" >=> listFolders ()
                route "/files" >=> Successful.NO_CONTENT
            ]
        POST >=> route "/upload" >=> Successful.NO_CONTENT
    ]