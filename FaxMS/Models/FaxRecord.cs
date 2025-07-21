using System;

namespace FaxMS.Models
{
    /// <summary>
    /// 傳真紀錄
    /// </summary>
    public class FaxRecord
    {
        public int Id { get; set; }
        /// <summary>
        /// PDF檔案路徑
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 傳真號碼
        /// </summary>
        public string FaxNumber { get; set; }
        /// <summary>
        /// 來源系統Id
        /// </summary>
        public int SourceSystemId { get; set; }
        public SourceSystem SourceSystem { get; set; }
        /// <summary>
        /// 險種
        /// </summary>
        public EnumInsurenceType InsurenceType { get; set; }
        /// <summary>
        /// 部門代碼
        /// </summary>
        public string DepartmentCode { get; set; }
        /// <summary>
        /// 傳送帳號
        /// </summary>
        public string? SenderAccount { get; set; }
        /// <summary>
        /// 接收時間
        /// </summary>
        public DateTime ReceivedAt { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 建立帳號
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// 修改帳號
        /// </summary>
        public string UpdatedBy { get; set; }
        /// <summary>
        /// 調閱人員
        /// </summary>
        public string? ViewedBy { get; set; }
        /// <summary>
        /// 調閱時間
        /// </summary>
        public DateTime? ViewedAt { get; set; }
    }
}