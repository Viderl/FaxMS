using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FaxMS.Models
{
    /// <summary>
    /// 來源系統
    /// </summary>
    public class SourceSystem
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 系統名稱
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 系統描述
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }

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

        /// <summary>
        /// IP清單
        /// </summary>
        public ICollection<SourceSystemIP> IPs { get; set; }
    }
}
