using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaxMS.Models
{
    /// <summary>
    /// 來源系統IP
    /// </summary>
    public class SourceSystemIP
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 來源系統Id
        /// </summary>
        [Required]
        public int SourceSystemId { get; set; }

        /// <summary>
        /// IP位址
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string IP { get; set; }

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

        [ForeignKey("SourceSystemId")]
        public SourceSystem SourceSystem { get; set; }
    }
}
