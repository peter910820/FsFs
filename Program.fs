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
open Routers.RootRouter
open Handlers.ErrorHandler

// ---------------------------------
// Main
// ---------------------------------

let configureCors (builder: CorsPolicyBuilder) =
    builder.WithOrigins(config.AllowCors).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
    |> ignore

let configureApp (app: IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()

    let app =
        if env.IsDevelopment() then
            app.UseDeveloperExceptionPage()
        else
            app.UseGiraffeErrorHandler(errorHandler)
               .UseHttpsRedirection()

    app.UseCors(configureCors)
       .UseStaticFiles()
        .UseGiraffe
        webApp

let configureServices (services: IServiceCollection) =
    let connectionString =
        [
            $"Host={config.DbHost}"
            $"Username={config.DbUsername}"
            $"Password={config.DbPassword}"
            $"Database={config.DbName}"
            $"Maximum Pool Size={config.DbMaxPoolSize}"
        ]
        |> String.concat ";"

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
