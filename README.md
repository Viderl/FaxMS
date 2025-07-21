# 傳真管理系統 FaxMS

## 專案簡介
本系統為 C# ASP.NET Core 8 MVC 傳真管理平台，支援 AD/LDAP 驗證、API 傳真接收、權限控管、Hangfire 排程、NLog 日誌、Swagger 文件，資料庫支援 SQLite/SQL Server。

## 主要功能
- AD/LDAP 驗證登入（僅允許 AD 使用者）
- 管理員/一般使用者權限分級
- API 傳真接收（PDF、號碼、來源系統、險種、部門等驗證）
- 傳真紀錄查詢、上傳、查閱紀錄
- 來源系統與 IP 管理
- 管理員帳號管理
- 部門每日自動同步
- 傳真檔案與紀錄自動清理（5年以上）
- 日誌記錄帳號與IP
- Swagger API 文件（繁體中文）

## 安裝與啟動
1. 安裝 .NET 8 SDK
2. 還原 NuGet 套件  
   `dotnet restore`
3. 建立資料庫  
   `dotnet ef database update`
4. 啟動專案  
   `dotnet run --project FaxMS`
5. 開發環境預設使用 SQLite，正式環境使用 SQL Server，連線字串於 `appsettings.json` 設定

## 重要設定
- LDAP 參數於 `appsettings.json` 設定
- 傳真檔案儲存路徑於 `appsettings.json` 設定
- Swagger 文件路徑 `/swagger`
- Hangfire Dashboard `/hangfire`（僅管理員可見）

## 權限說明
- 管理員可管理所有資料與設定
- 一般使用者僅可查詢與上傳自己傳真
- 管理員帳號需於管理介面設定啟用

## API 文件
請參閱 `/swagger` 取得繁體中文 API 文件。

## 注意事項
- 本系統僅支援繁體中文
- 所有敏感資料皆經加密處理
- 請定期備份資料庫與檔案
