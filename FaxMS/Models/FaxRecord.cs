using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaxMS.Models
{
    /// <summary>
    /// 傳真紀錄
    /// </summary>
    public class FaxRecord
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 傳真號碼
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string FaxNumber { get; set; }

        /// <summary>
        /// PDF檔案路徑
        /// </summary>
        [Required]
        public string FilePath { get; set; }

        /// <summary>
        /// 傳真號碼TXT檔案路徑
        /// </summary>
        [Required]
        public string TxtPath { get; set; }

        /// <summary>
        /// 來源系統
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string SourceSystem { get; set; }

        /// <summary>
        /// 險種
        /// </summary>
        [Required]
        public EnumInsurenceType InsuranceType { get; set; }

        /// <summary>
        /// 部門
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Department { get; set; }

        /// <summary>
        /// 傳送帳號
        /// </summary>
        [MaxLength(100)]
        public string SenderAccount { get; set; }

        /// <summary>
        /// 傳真接收時間
        /// </summary>
        [Required]
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// 狀態（如已完成、處理中）
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; }

        /// <summary>
        /// 完成時間
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 完成後PDF檔案路徑
        /// </summary>
        public string CompleteFilePath { get; set; }

        /// <summary>
        /// 完成後TXT檔案路徑
        /// </summary>
        public string CompleteTxtPath { get; set; }

        /// <summary>
        /// 查閱紀錄
        /// </summary>
        public ICollection<FaxViewLog> ViewLogs { get; set; }
    }
}
