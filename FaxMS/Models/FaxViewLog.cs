using System;

namespace FaxMS.Models
{
    /// <summary>
    /// 傳真調閱紀錄
    /// </summary>
    public class FaxViewLog
    {
        public int Id { get; set; }
        /// <summary>
        /// 傳真紀錄Id
        /// </summary>
        public int FaxRecordId { get; set; }
        public FaxRecord FaxRecord { get; set; }
        /// <summary>
        /// 調閱人員
        /// </summary>
        public string ViewedBy { get; set; }
        /// <summary>
        /// 調閱時間
        /// </summary>
        public DateTime ViewedAt { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 建立帳號
        /// </summary>
        public string CreatedBy { get; set; }
    }
}