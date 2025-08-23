module SeaottermsSiteFileserver.Routers.ApiRouter

open Giraffe

let apiRoutes : HttpHandler =
    choose [
        GET >=>
            choose [
                route "/directory" >=> Successful.NO_CONTENT
                route "/files" >=> Successful.NO_CONTENT
            ]
        POST >=> route "/upload" >=> Successful.NO_CONTENT
    ]