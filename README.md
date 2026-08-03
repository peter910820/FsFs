# FsFs

以 F# 與 [Giraffe](https://github.com/giraffe-fsharp/Giraffe) 打造的個人檔案伺服器，搭配 Vue 3 前端，提供檔案瀏覽、上傳與管理功能。

後端與前端為分離式架構；部署時前端與靜態資源通常由 Nginx 代理，後端只負責 API。

## 功能

- 瀏覽資料夾與檔案列表
- 登入後上傳檔案、建立資料夾、刪除檔案
- 靜態檔案服務（Nginx 代理或內建 `Manual` 模式）
- Markdown 預覽與語法高亮（前端）

## 技術棧


| 層級  | 技術                                              |
| --- | ----------------------------------------------- |
| 後端  | F#、Giraffe、ASP.NET Core 8、Entity Framework Core |
| 資料庫 | PostgreSQL                                      |
| 認證  | Cookie `sid` + MemoryCache session（BCrypt 驗證密碼） |
| 前端  | Vue 3、TypeScript、Vite、Vuetify                    |




## 專案結構

```
FsFs/
├── Handlers/          # API 處理邏輯
├── Infrastructure/    # 設定、資料庫、中介層、回應工廠
├── Models/            # DB / DTO 模型
├── Routers/           # 路由定義
├── Tests/             # 單元測試
├── Frontend/          # Vue 前端（獨立專案）
├── Program.fs         # 應用程式入口
└── .env.example       # 後端環境變數範本
```



## 環境需求

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)（建議搭配 Yarn）
- PostgreSQL（需有 `users` 資料表）



## 快速開始



### 後端

1. 複製並填寫環境變數：
  ```bash
   cp .env.example .env
  ```
2. 啟動：
  ```bash
   dotnet run
  ```

啟動時會檢查資料庫連線。

### 前端

```bash
cd Frontend
cp .env.example .env
yarn install
yarn dev
```



## 環境變數



### 後端（`.env`）


| 變數                                                    | 說明                                                           |
| ----------------------------------------------------- | ------------------------------------------------------------ |
| `DB_HOST` / `DB_USERNAME` / `DB_PASSWORD` / `DB_NAME` | PostgreSQL 連線                                                |
| `DB_MAX_POOL_SIZE`                                    | 連線池上限                                                        |
| `RUNTIME_PORT`                                        | 監聽位址，例如 `http://127.0.0.1:3023`                              |
| `IS_PRODUCTION`                                       | `false` 本機／`true` 生產；決定 cookie 的 Secure、SameSite、是否套用 Domain |
| `DOMAIN`                                              | Cookie Domain；**僅** `IS_PRODUCTION=true` 時必填                 |
| `ALLOW_CORS`                                          | 允許的 CORS Origin                                              |
| `CONTENT_ROOT`                                        | 檔案根目錄（實際存放上傳／列表的路徑）                                          |
| `START_MODE`                                          | `NGINX`：靜態檔由外部代理；`Manual`：後端掛載 `/resource/*`                 |
| `API_TOKENS`                                          | 逗號分隔的 Bearer tokens；供 server 上傳 API 使用（可留空）                    |




### Cookie 行為（由 `IS_PRODUCTION` 自動決定）


| `IS_PRODUCTION` | Secure  | SameSite | Domain      |
| --------------- | ------- | -------- | ----------- |
| `false`         | `false` | `Lax`    | 不設定         |
| `true`          | `true`  | `None`   | 使用 `DOMAIN` |


 `ALLOW_CORS` 需設定前端 Origin。

### 前端（`Frontend/.env`）


| 變數                        | 說明                                   |
| ------------------------- | ------------------------------------ |
| `VITE_API_DOMAIN`         | 後端 API 基底 URL                        |
| `VITE_STATIC_FILE_DOMAIN` | 靜態檔案基底 URL                           |
| `VITE_OG_*`               | 生產環境 Open Graph 相關（見 `.env.example`） |




## API 概要

所有 API 前綴為 `/api`。需登入的端點檢查 Cookie `sid`；server 上傳端點改用 `Authorization: Bearer`（`API_TOKENS`）。


| 方法       | 路徑                             | 認證         | 說明                                      |
| -------- | ------------------------------ | ---------- | --------------------------------------- |
| `GET`    | `/api/directories`             | 否          | 列出資料夾                                   |
| `GET`    | `/api/files`                   | 否          | 列出檔案；可選 `?dir=`                         |
| `GET`    | `/api/files/recent`            | 否          | 最近檔案（最多 10 筆；可選 `?limit=`）              |
| `POST`   | `/api/login`                   | 否          | 登入，設定 `sid`                             |
| `POST`   | `/api/auth`                    | Cookie     | 驗證 session 是否有效                         |
| `POST`   | `/api/upload/{dir}`            | Cookie     | 上傳檔案到指定目錄（multipart）                    |
| `POST`   | `/api/server/upload/{dir}`     | Bearer     | server端上傳（`fileName` + `contentBase64`） |
| `POST`   | `/api/create-directory/{name}` | Cookie     | 建立資料夾                                   |
| `DELETE` | `/api/file`                    | Cookie     | 刪除檔案（JSON body：`fileName`）              |


當 `START_MODE=Manual` 時，另提供 `/resource/{code\|image\|technology\|test\|test2}/...` 靜態檔路由。

回應格式大致為：

```json
{
  "statusCode": 200,
  "msg": "...",
  "data": {}
}
```

`data` 無內容時為 `null`。

## 測試

```bash
dotnet test Tests/FsFs.Tests.fsproj
```



## 授權

[MIT](LICENSE)