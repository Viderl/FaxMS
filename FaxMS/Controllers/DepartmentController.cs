using Microsoft.AspNetCore.Mvc;
using FaxMS.Data;
using FaxMS.Models;
using System.Linq;

namespace FaxMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 查詢部門快照清單
        /// </summary>
        [HttpGet("list")]
        public IActionResult List()
        {
            var departments = _db.DepartmentSnapshots
                .OrderByDescending(d => d.SyncedAt)
                .ToList();
            return Ok(departments);
        }
    }
}