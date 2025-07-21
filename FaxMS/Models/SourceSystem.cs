using System.Collections.Generic;
using System;

namespace FaxMS.Models
{
    /// <summary>
    /// 來源系統表
    /// </summary>
    public class SourceSystem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SourceSystemIP> IPs { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class SourceSystemIP
    {
        public int Id { get; set; }
        public string IP { get; set; } = string.Empty;
        public int SourceSystemId { get; set; }
        public SourceSystem? SourceSystem { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
