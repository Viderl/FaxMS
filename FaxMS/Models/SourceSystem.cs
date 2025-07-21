using System;
using System.Collections.Generic;

namespace FaxMS.Models
{
    /// <summary>
    /// 來源系統資料表
    /// </summary>
    public class SourceSystem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public ICollection<SourceSystemIP> IPs { get; set; } = new List<SourceSystemIP>();
    }
}