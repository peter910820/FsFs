module SeaottermsSiteFileserver.App

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

open Config
open DbConnect
open Routers.ApiRouter
open Routers.ResourceRouter
open Handlers.StaticFileHandler

// ---------------------------------
// Web app
// ---------------------------------

let webApp =
    choose [
        subRoute "/resource" staticFileRoutes
        subRoute "/api" apiRoutes
        GET >=>
            choose [
                // SPA static file
                routef "/assets/%s" (serveStaticFile (Path.Combine(rootDir, "dist", "assets")))
                htmlFile (Path.Combine(rootDir, "dist", "index.html"))
            ]
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder
        // .WithOrigins(
        //     "http://localhost:5000",
        //     "https://localhost:5001")
       .AllowAnyMethod()
       .AllowAnyHeader()
       |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    (match env.IsDevelopment() with
    | true  ->
        app.UseDeveloperExceptionPage()
    | false ->
        app .UseGiraffeErrorHandler(errorHandler)
            .UseHttpsRedirection())
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    let connectionString = "Host=localhost;Username=postgres;Password=1234;Database=mydb;Maximum Pool Size=20"

    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

    services.AddDbContextPool<AppDbContext>(fun options ->
    options.UseNpgsql(connectionString) |> ignore
    ) |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main args =
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .UseUrls("http://127.0.0.1:3052")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseWebRoot(rootDir)
                    .Configure(Action<IApplicationBuilder> configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging configureLogging 
                    |> ignore)
        .Build()
        .Run()
    0