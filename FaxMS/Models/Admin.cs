using System;

namespace FaxMS.Models
{
    /// <summary>
    /// 管理員
    /// </summary>
    public class Admin
    {
        public int Id { get; set; }
        /// <summary>
        /// AD帳號
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 啟用狀態
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 加入時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 加入帳號
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