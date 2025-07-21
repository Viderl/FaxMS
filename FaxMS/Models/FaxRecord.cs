using System;

namespace FaxMS.Models
{
    /// <summary>
    /// 傳真紀錄表
    /// </summary>
    public class FaxRecord
    {
        public int Id { get; set; }
        public string FaxNumber { get; set; } = string.Empty;
        public DateTime FaxTime { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string SourceSystem { get; set; } = string.Empty;
        public string SourceIP { get; set; } = string.Empty;
        public string InsurenceType { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string? Account { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    /// <summary>
    /// 傳真調閱紀錄表
    /// </summary>
    public class FaxRecordAccess
    {
        public int Id { get; set; }
        public int FaxRecordId { get; set; }
        public FaxRecord? FaxRecord { get; set; }
        public string Account { get; set; } = string.Empty;
        public DateTime AccessedAt { get; set; }
    }
}
