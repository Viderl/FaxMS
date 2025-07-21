using System;

namespace FaxMS.Models
{
    /// <summary>
    /// 傳真紀錄資料表
    /// </summary>
    public class Fax
    {
        public int Id { get; set; }
        public string FaxNumber { get; set; } = string.Empty;
        public DateTime ReceivedAt { get; set; }
        public string SourceSystem { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string InsuranceType { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string TxtPath { get; set; } = string.Empty;
        public string? SenderAccount { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? CompletedFilePath { get; set; }
        public string? CompletedTxtPath { get; set; }
        public string? ViewAccount { get; set; }
        public DateTime? ViewTime { get; set; }
        public string? ViewIp { get; set; }
    }
}