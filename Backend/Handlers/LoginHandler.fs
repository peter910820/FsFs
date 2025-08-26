module SeaottermsSiteFileserver.Handlers.LoginHandler

open System.IO
open Giraffe

open SeaottermsSiteFileserver.Config

let loginHandler () : HttpHandler =
     fun next ctx ->
        task {
            let handler = setStatusCode 200

            return! handler next ctx
        }