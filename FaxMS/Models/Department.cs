using System;
using System.ComponentModel.DataAnnotations;

namespace FaxMS.Models
{
    /// <summary>
    /// 部門清單
    /// </summary>
    public class Department
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 部門代碼
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

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
