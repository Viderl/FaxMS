using System;

namespace FaxMS.Models
{
    /// <summary>
    /// 管理員帳號表
    /// </summary>
    public class Admin
    {
        public int Id { get; set; }
        public string Account { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
