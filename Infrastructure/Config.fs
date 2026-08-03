module FsFs.Infrastructure.Config

open System
open dotenv.net
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Caching.Memory

// ----------------------------
// Config以及MemoryCache 初始化， 整個檔案都是以產生副作用的前提下運作的
// ----------------------------

DotEnv.Load()

// ----------------------------
// MemoryCache 初始化
// ----------------------------
let cache = new MemoryCache(MemoryCacheOptions())


type AppConfig =
    { DbHost: string
      DbUsername: string
      DbPassword: string
      DbName: string
      DbMaxPoolSize: string
      ContentRoot: string
      RuntimePort: string
      Domain: string option
      IsProduction: bool
      CookieSecure: bool
      CookieSameSite: SameSiteMode
      AllowCors: string
      StartMode: string
      ApiTokens: string list }

let private checkEnvKey key =
    match Environment.GetEnvironmentVariable key with
    | null -> failwithf "Missing required env var: %s" key
    | v -> v

let private parseBool key =
    match (checkEnvKey key).Trim().ToLowerInvariant() with
    | "true"
    | "1"
    | "yes" -> true
    | "false"
    | "0"
    | "no" -> false
    | v -> failwithf "Invalid bool for %s: %s (use true/false)" key v

let private optionalEnv key =
    match Environment.GetEnvironmentVariable key with
    | v when String.IsNullOrWhiteSpace v -> None
    | v -> Some v

/// <summary>將逗號分隔的 API tokens 轉成 list（trim、略過空白）</summary>
let parseApiTokens (raw: string) : string list =
    if String.IsNullOrWhiteSpace raw then
        []
    else
        raw.Split(',')
        |> Array.map (fun t -> t.Trim())
        |> Array.filter (fun t -> not (String.IsNullOrWhiteSpace t))
        |> Array.toList

let config =
    let isProduction = parseBool "IS_PRODUCTION"
    let domain = optionalEnv "DOMAIN"

    if isProduction && domain.IsNone then
        failwith "DOMAIN is required when IS_PRODUCTION=true"

    let apiTokens =
        match Environment.GetEnvironmentVariable "API_TOKENS" with
        | null -> []
        | v -> parseApiTokens v

    { DbHost = checkEnvKey "DB_HOST"
      DbUsername = checkEnvKey "DB_USERNAME"
      DbPassword = checkEnvKey "DB_PASSWORD"
      DbName = checkEnvKey "DB_NAME"
      DbMaxPoolSize = checkEnvKey "DB_MAX_POOL_SIZE"
      ContentRoot = checkEnvKey "CONTENT_ROOT"
      RuntimePort = checkEnvKey "RUNTIME_PORT"
      Domain = domain
      IsProduction = isProduction
      CookieSecure = isProduction
      CookieSameSite = if isProduction then SameSiteMode.None else SameSiteMode.Lax
      AllowCors = checkEnvKey "ALLOW_CORS"
      StartMode = checkEnvKey "START_MODE"
      ApiTokens = apiTokens }
