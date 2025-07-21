using System;

namespace FaxMS.Models
{
    /// <summary>
    /// 部門快照
    /// </summary>
    public class DepartmentSnapshot
    {
        public int Id { get; set; }
        /// <summary>
        /// 部門代碼
        /// </summary>
        public string DepartmentCode { get; set; }
        /// <summary>
        /// 部門名稱
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 同步時間
        /// </summary>
        public DateTime SyncedAt { get; set; }
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
    }
}