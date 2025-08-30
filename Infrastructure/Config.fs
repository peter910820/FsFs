module FsFs.Infrastructure.Config

open System
open dotenv.net
open Microsoft.Extensions.Caching.Memory

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
      Domain: string }

let private checkEnvKey key =
    match Environment.GetEnvironmentVariable key with
    | null -> failwithf "Missing required env var: %s" key
    | v -> v

let config =
    { DbHost = checkEnvKey "DB_HOST"
      DbUsername = checkEnvKey "DB_USERNAME"
      DbPassword = checkEnvKey "DB_PASSWORD"
      DbName = checkEnvKey "DB_NAME"
      DbMaxPoolSize = checkEnvKey "DB_MAX_POOL_SIZE"
      ContentRoot = checkEnvKey "CONTENT_ROOT"
      RuntimePort = checkEnvKey "RUNTIME_PORT"
      Domain = checkEnvKey "DOMAIN"}
