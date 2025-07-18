# 傳真管理系統(Fax Management System)
## 系統需求
- 使用C# ASP.NET Core 開發，不另外使用前端框架
- 使用Entity Framework Core作為ORM框架，本機開發時使用SQLite作為資料庫，正式環境使用SQL Server作為資料庫
- 使用Hangfire進行排程任務管理
- 使用Swagger進行API文件生成
- 使用NLog進行日誌記錄
- 管理介面對只允許AD使用者透過LDAP驗證登入。
- 傳真接收方式是透過API接收PDF檔案和傳真號碼，並將PDF檔案存到指定目錄，傳真號碼儲存成與pdf檔名相同的檔案，但是附檔名是.txt。
- 傳真完成的檔案會移到另一個目錄。系統可從另一個目錄調閱傳真資料
- 基本權限控制
    - 管理員可以查看所有人傳真資料，設定系統管理員角色
    - 使用者只能傳真和查看自己的傳真資料
## 開發注意事項
- 確保所有API都使用HTTPS協議
- 所有API都應有適當的錯誤處理和日誌記錄
- 所有敏感資料（如密碼）應加密存儲
## 系統參數
- 險種表
```
    public enum EnumInsurenceType
    {
        /// <summary>
        /// 傷害險
        /// </summary>
        A,
        /// <summary>
        /// 商火
        /// </summary>
        B,
        /// <summary>
        /// 車險
        /// </summary>
        C,
        /// <summary>
        /// 住火
        /// </summary>
        F,
        /// <summary>
        /// 健康險
        /// </summary>
        H,
        /// <summary>
        /// 水險
        /// </summary>
        M,
        /// <summary>
        /// 新種險
        /// </summary>
        O
    }
```
- 部門透過API取得。URL:https://api-test.tmnewa.com.tw/ebp/common/!Partner/TMNewa.Mis.Partner.Service.AIOuterService.svc/json/GetTMNewaDepartment
## 介面
- 提供簡單的前端介面，允許使用者傳真PDF檔案和查看傳真記錄。
- 管理介面允許系統管理員查看所有傳真記錄和設定角色。
- 帳號透過LDAP查詢帳號是否存在，並使用AD驗證登入。
## API
- 傳真接收API。接收參數
    1. pdf檔案，必填
    2. 傳送號碼，必填
    3. 來源系統，必填
    4. 分攤險種，必填
    5. 分攤部門，必填
    6. 傳送帳號，選填
## 背景作業
- 使用Hangfire定期清理超過5年的傳真記錄和檔案。
- 每天1點同步部門清單
