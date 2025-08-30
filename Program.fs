module FsFs.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Microsoft.EntityFrameworkCore
open Giraffe

open Infrastructure.Config
open Infrastructure.Database
open Routers.ApiRouter
open Routers.ResourceRouter
open Handlers.ErrorHandler
open FsFs.Infrastructure.ResponseFactory

// ---------------------------------
// Web app
// ---------------------------------

let webApp =
    choose
        [ subRoute "/resource" staticFileRoutes
          subRoute "/api" apiRoutes
          RequestErrors.notFound (responseFactory 404 "not found" null) ]

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder: CorsPolicyBuilder) =
    builder.WithOrigins(config.AllowCors).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
    |> ignore

let configureApp (app: IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()

    (match env.IsDevelopment() with
     | true -> app.UseDeveloperExceptionPage()
     | false -> app.UseGiraffeErrorHandler(errorHandler).UseHttpsRedirection())
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe
        webApp

let configureServices (services: IServiceCollection) =
    let connectionString =
        sprintf
            "Host=%s;Username=%s;Password=%s;Database=%s;Maximum Pool Size=%s"
            config.DbHost
            config.DbUsername
            config.DbPassword
            config.DbName
            config.DbMaxPoolSize

    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore

    services.AddDbContextPool<AppDbContext>(fun options -> options.UseNpgsql connectionString |> ignore)
    |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder.AddConsole().AddDebug() |> ignore

[<EntryPoint>]
let main args =
    let host =
        Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(fun webHostBuilder ->
                webHostBuilder
                    .UseUrls(config.RuntimePort)
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseWebRoot(config.ContentRoot)
                    .Configure(Action<IApplicationBuilder> configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging(configureLogging)
                    .ConfigureKestrel(fun options -> options.Limits.MaxRequestBodySize <- 50L * 1024L * 1024L) // 50 MB
                |> ignore)
            .Build()

    // 立即執行SQL連線，若連線失敗會直接關閉程式
    checkDbConnection host.Services
    |> Async.RunSynchronously
    |> function
        | Ok() -> printfn "✅ Database connection successful."
        | Error msg ->
            printfn "❌ Database connection failed: %s" msg
            Environment.Exit 1

    host.Run()
    0
