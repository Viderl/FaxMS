using System;
using System.Collections.Generic;

namespace FaxMS.Models
{
    /// <summary>
    /// 來源系統
    /// </summary>
    public class SourceSystem
    {
        public int Id { get; set; }
        /// <summary>
        /// 系統名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 啟用狀態
        /// </summary>
        public bool IsActive { get; set; }
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
        /// 對應多個 IP
        /// </summary>
        public List<SourceIp> SourceIps { get; set; }
    }

    /// <summary>
    /// 來源系統 IP
    /// </summary>
    public class SourceIp
    {
        public int Id { get; set; }
        public int SourceSystemId { get; set; }
        public string Ip { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public SourceSystem SourceSystem { get; set; }
    }
}