using System;
using System.ComponentModel.DataAnnotations;

namespace FaxMS.Models
{
    /// <summary>
    /// 管理員清單
    /// </summary>
    public class AdminUser
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 管理員帳號
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Account { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 建立帳號
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// 修改帳號
        /// </summary>
        [MaxLength(100)]
        public string ModifiedBy { get; set; }
    }
}
