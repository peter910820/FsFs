# FsFs

以 F# 與 [Giraffe](https://github.com/giraffe-fsharp/Giraffe) 打造的個人檔案伺服器，搭配 Vue 3 前端，提供檔案瀏覽、上傳與管理功能。

## 功能

- 瀏覽資料夾與檔案列表
- 登入驗證後上傳檔案、建立資料夾、刪除檔案
- 靜態檔案服務（支援 Nginx 代理或內建模式）
- Markdown 預覽與語法高亮

## 技術棧

| 層級 | 技術 |
|------|------|
| 後端 | F#、Giraffe、ASP.NET Core 8、Entity Framework Core |
| 資料庫 | PostgreSQL |
| 前端 | Vue 3、TypeScript、Vite、Pinia、Materialize CSS |

## 專案結構

```
FsFs/
├── Handlers/          # API 處理邏輯
├── Infrastructure/    # 設定、資料庫、中介層
├── Models/            # 資料模型
├── Routers/           # 路由定義
├── Tests/             # 單元測試
└── Frontend/          # Vue 前端（獨立專案）
```

後端與前端為分離式架構，部署時前端通常由 Nginx 代理至後端 API。

## 快速開始

### 環境需求

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)（建議搭配 Yarn）
- PostgreSQL

### 後端

1. 複製環境設定並填入資料：

   ```bash
   cp .env.example .env
   ```

2. 啟動後端：

   ```bash
   dotnet run
   ```

### 前端

1. 進入前端目錄並安裝依賴：

   ```bash
   cd Frontend
   cp .env.example .env
   yarn install
   ```

2. 啟動開發伺服器：

   ```bash
   yarn dev
   ```

## 授權

[MIT](LICENSE)
