using Microsoft.AspNetCore.Mvc;
using FaxMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FaxMS.Controllers
{
    public class FaxRecordsController : Controller
    {
        private readonly FaxDbContext _db;

        public FaxRecordsController(FaxDbContext db)
        {
            _db = db;
        }

        // GET: /FaxRecords/
        public async Task<IActionResult> Index(string faxNumber, string date)
        {
            var query = _db.Faxes.AsQueryable();
            if (!string.IsNullOrEmpty(faxNumber))
                query = query.Where(f => f.FaxNumber.Contains(faxNumber));
            if (!string.IsNullOrEmpty(date) && System.DateTime.TryParse(date, out var dt))
                query = query.Where(f => f.ReceivedAt.Date == dt.Date);

            // 權限控管：管理員可看全部，一般使用者僅看自己
            var account = User.Identity?.Name ?? "";
            var isAdmin = false; // TODO: 依管理員表判斷
            if (!isAdmin)
                query = query.Where(f => f.SenderAccount == account);

            var list = await query.OrderByDescending(f => f.ReceivedAt).ToListAsync();
            return View(list);
        }

        // GET: /FaxRecords/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: /FaxRecords/Upload
        [HttpPost]
        public async Task<IActionResult> Upload(FaxUploadViewModel model)
        {
            if (model.PdfFile == null || model.PdfFile.Length == 0)
            {
                ViewBag.Error = "請選擇PDF檔案";
                return View();
            }
            // 其餘驗證略，與API類似
            // 儲存檔案與寫入資料庫略
            ViewBag.Message = "上傳成功";
            return View();
        }
    }

    public class FaxUploadViewModel
    {
        public Microsoft.AspNetCore.Http.IFormFile PdfFile { get; set; }
        public string FaxNumber { get; set; }
        public string InsuranceType { get; set; }
        public string Department { get; set; }
    }
}