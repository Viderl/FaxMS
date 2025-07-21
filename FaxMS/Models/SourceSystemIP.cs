using System;

namespace FaxMS.Models
{
    /// <summary>
    /// 來源系統IP資料表
    /// </summary>
    public class SourceSystemIP
    {
        public int Id { get; set; }
        public int SourceSystemId { get; set; }
        public string IP { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public SourceSystem? SourceSystem { get; set; }
    }
}