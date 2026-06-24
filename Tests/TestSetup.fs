module FsFs.Tests.TestSetup

// FsFs.Infrastructure.Config 在載入時會讀取環境變數，測試啟動前先設定假值。
let private setEnv key value =
    System.Environment.SetEnvironmentVariable(key, value)

do
    setEnv "DB_HOST" "localhost"
    setEnv "DB_USERNAME" "test"
    setEnv "DB_PASSWORD" "test"
    setEnv "DB_NAME" "test"
    setEnv "DB_MAX_POOL_SIZE" "10"
    setEnv "CONTENT_ROOT" (System.IO.Path.GetTempPath())
    setEnv "RUNTIME_PORT" "5432"
    setEnv "DOMAIN" "localhost"
    setEnv "ALLOW_CORS" "*"
    setEnv "START_MODE" "Manual"
