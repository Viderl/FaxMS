using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaxMS.Models
{
    /// <summary>
    /// 傳真查閱紀錄
    /// </summary>
    public class FaxViewLog
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 傳真紀錄Id
        /// </summary>
        [Required]
        public int FaxRecordId { get; set; }

        /// <summary>
        /// 查閱帳號
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Account { get; set; }

        /// <summary>
        /// 查閱時間
        /// </summary>
        [Required]
        public DateTime ViewTime { get; set; }

        [ForeignKey("FaxRecordId")]
        public FaxRecord FaxRecord { get; set; }
    }
}
