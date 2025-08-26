module SeaottermsSiteFileserver.Config

open System
open System.IO

// 根路由設定
let rootDir = 
        // 輸入根目錄的WebRoot路徑
        match Environment.GetEnvironmentVariable "CONTENT_ROOT" with
        | null | "" -> Path.Combine(Directory.GetCurrentDirectory(), "WebRoot")
        | path -> path 