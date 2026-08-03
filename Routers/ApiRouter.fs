module FsFs.Routers.ApiRouter

open Giraffe

open FsFs.Handlers.DirectoryHandler
open FsFs.Handlers.FileHandler
open FsFs.Handlers.UploadHandler
open FsFs.Handlers.ServerUploadHandler
open FsFs.Handlers.LoginHandler
open FsFs.Handlers.AuthHandler
open FsFs.Handlers.CreateDirectoryHandler
open FsFs.Infrastructure.Middleware

/// <summary>API主路由</summary>
let apiRoutes : HttpHandler =
    choose
        [ GET
          >=> choose
                  [ route "/directories" >=> listFolders
                    route "/files/recent" >=> listRecentFiles
                    route "/files" >=> listFile ]
          POST
          >=> choose
                  [ route "/auth" >=> authMiddleware >=> authHandler
                    route "/login" >=> loginHandler
                    routef "/upload/%s" (fun fileName -> authMiddleware >=> uploadHandler fileName)
                    routef "/server/upload/%s" (fun dir -> apiTokenMiddleware >=> serverUploadHandler dir)
                    routef "/create-directory/%s" (fun dirName -> authMiddleware >=> createDirectoryHandler dirName) ]
          DELETE >=> choose [ route "/file" >=> authMiddleware >=> deleteFileHandler ] ]
