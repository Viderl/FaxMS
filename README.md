# 傳真管理系統(Fax Management System)
## 系統需求
- 使用C# ASP.NET MVC Core Net 8開發，不另外使用前端框架
- 使用Entity Framework Core作為ORM框架，本機開發時使用SQLite作為資料庫，正式環境使用SQL Server作為資料庫
- 使用Hangfire進行排程任務管理
- 使用Swagger進行API文件生成
- 使用NLog進行日誌記錄
- 管理畫面使用瀏覽器登入
- 管理畫面對只允許AD使用者透過LDAP驗證登入。
- 傳真接收方式是透過API接收PDF檔案和傳真號碼，並將PDF檔案存到指定目錄，傳真號碼儲存成與pdf檔名相同的檔案，但是附檔名是.txt。存檔完畢會由另一隻程式完成傳真。本系統不包含執行傳真的程式。
- 系統紀錄每次收到傳真的時間
- 系統紀錄調閱傳真的人員、時間傳真編號
- 傳真完成的檔案會移到另一個目錄。系統可從另一個目錄調閱傳真資料
- API只用限制IP方式限制來源，IP存在資料庫
- 系統畫面只使用繁體中文和英文(如果需要)
- 基本權限控制
    - 管理員可以查看所有人傳真資料，設定系統管理員角色
    - 管理員可以使用來源系統管理介面
    - 一般使用者只能傳真和查看自己的傳真資料
    - 只要能成功透過LDAP驗證登入，一定是一般使用者。管理員必須透過介面設定
## 開發注意事項
- 確保所有API都使用HTTPS協議
- 所有API都應有適當的錯誤處理和日誌記錄
- 所有敏感資料（如密碼）應加密存儲
- 不需要User這個Table，只需要一個Table存誰是管理員，何時加入、停用。
- 不需要Services
- 只需要一個專案，不需要分拆
- 除了傳真紀錄，每個資料表都要記錄建立時間、建立帳號、修改時間、修改帳號
- 錯誤訊息和swagger document僅需繁體中文 
- 日誌需包含操作帳號與IP
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
- 部門透過API取得，無特殊驗證方式。URL:https://api-test.tmnewa.com.tw/ebp/common/!Partner/TMNewa.Mis.Partner.Service.AIOuterService.svc/json/GetTMNewaDepartment
## 介面
1. 登入畫面。帳號透過LDAP查詢帳號是否存在，並使用AD驗證登入。Controller直接呼叫資料庫，不需要透過API。
2. 紀錄查詢介面，支援條件搜尋，搜尋條件有傳真號碼、傳真時間。允許使用者傳真PDF檔案和查看傳真記錄。登入後預設到此畫面。所有人都可以開啟此介面。管理員允許查看所有傳真紀錄，其他人只能查看自己的傳真紀錄。Controller直接呼叫資料庫，不需要透過API。
3. 來源系統管理介面。設定來源系統和IP，一個系統可能會有多個IP。只有管理員可以開啟此介面。Controller直接呼叫資料庫，不需要透過API。
4. 管理員管理介面。編輯管理員帳號。只有管理員可以開啟此介面。Controller直接呼叫資料庫，不需要透過API。
## API
- 只需要接收傳真API。接收參數如下。API收到PDF檔，將PDF放到兩個地方，一個是傳真APP處理的資料夾，此資料夾路徑固定，寫在config。另一個是管理系統本身使用的資料夾，用以調閱。資料夾用寫在config內的一個固定路徑加上/yyyy/mm/dd格式儲存，PDF檔名格式yyyymmddHHmmssffff。
    1. pdf檔案，必填，大小不超過3MB
    2. 傳送號碼，必填
    3. 來源系統，必填，且IP必須符合來源系統設定的IP
    4. 分攤險種，必填，且必須落在險種表中
    5. 分攤部門，必填，且必須落在現有部門中
    6. 傳送帳號，選填
  
## 背景作業
- 使用Hangfire定期清理超過5年的傳真記錄和檔案。
- 每天1點同步部門清單
