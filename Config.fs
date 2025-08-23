module SeaottermsSiteFileserver.Config

open System.IO

// 根路由設定
let rootDir = Path.Combine(Directory.GetCurrentDirectory(), "WebRoot")