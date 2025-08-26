module SeaottermsSiteFileserver.Routers.ApiRouter

open Giraffe

open SeaottermsSiteFileserver.Handlers.DirectoryHandler
open SeaottermsSiteFileserver.Handlers.FileHandler
open SeaottermsSiteFileserver.Handlers.UploadHandler
open SeaottermsSiteFileserver.Handlers.LoginHandler

let apiRoutes : HttpHandler =
    choose [
        GET >=>
            choose [
                route "/directories" >=> listFolders ()
                route "/files" >=> listFile ()
            ]
        POST >=> routef "/upload/%s" uploadHandler
        POST >=> route "login" >=> loginHandler ()
    ]