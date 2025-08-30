module FsFs.Routers.ApiRouter

open Giraffe

open FsFs.Handlers.DirectoryHandler
open FsFs.Handlers.FileHandler
open FsFs.Handlers.UploadHandler
open FsFs.Handlers.LoginHandler
open FsFs.Handlers.AuthHandler
open FsFs.Handlers.CreateDirectoryHandler

let apiRoutes: HttpHandler =
    choose
        [ GET
          >=> choose [ route "/directories" >=> listFolders (); route "/files" >=> listFile () ]
          POST >=> routef "/upload/%s" uploadHandler
          POST >=> route "/login" >=> loginHandler ()
          POST >=> route "/auth" >=> authHandler ()
          POST >=> route "/create-directory" >=> createDirectoryHandler () ]
