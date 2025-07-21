using Microsoft.AspNetCore.Mvc;
using FaxMS.Data;
using FaxMS.Models;

namespace FaxMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaxController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public FaxController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 傳真接收API
        /// </summary>
        /// <param name="file">PDF檔案</param>
        /// <param name="faxNumber">傳真號碼</param>
        /// <param name="sourceSystem">來源系統</param>
        /// <param name="insurenceType">分攤險種</param>
        /// <param name="departmentCode">分攤部門</param>
        /// <param name="senderAccount">傳送帳號</param>
        /// <returns></returns>
        [HttpPost("receive")]
        public IActionResult ReceiveFax(
            [FromForm] IFormFile file,
            [FromForm] string faxNumber,
            [FromForm] string sourceSystem,
            [FromForm] string insurenceType,
            [FromForm] string departmentCode,
            [FromForm] string? senderAccount)
        {
            // TODO: 驗證、檔案儲存、資料庫寫入
            return Ok("收到傳真");
        }
    }
}