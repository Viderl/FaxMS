using FaxMS.Data;
using FaxMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FaxMS.Controllers
{
    [Authorize]
    public class FaxUploadController : Controller
    {
        private readonly FaxMSDbContext _db;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public FaxUploadController(FaxMSDbContext db, IConfiguration config, IWebHostEnvironment env)
        {
            _db = db;
            _config = config;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFax(string faxNumber, string insurenceType, string department, IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0 || pdfFile.Length > 3 * 1024 * 1024)
            {
                ModelState.AddModelError("", "PDF 檔案必填且不可超過 3MB");
                return View("Index");
            }
            if (string.IsNullOrWhiteSpace(faxNumber) || string.IsNullOrWhiteSpace(insurenceType) || string.IsNullOrWhiteSpace(department))
            {
                ModelState.AddModelError("", "所有欄位皆必填");
                return View("Index");
            }
            // 險種驗證
            if (!Enum.TryParse<EnumInsurenceType>(insurenceType, out var _))
            {
                ModelState.AddModelError("", "險種不正確");
                return View("Index");
            }
            // 部門驗證（略，實務應查部門清單）
            // 儲存檔案
            var now = DateTime.Now;
            var fileName = now.ToString("yyyyMMddHHmmssffff", CultureInfo.InvariantCulture) + ".pdf";
            var txtName = Path.ChangeExtension(fileName, ".txt");
            var baseDir = _config["Fax:BaseDir"] ?? Path.Combine(_env.ContentRootPath, "FaxFiles");
            var datePath = Path.Combine(baseDir, now.ToString("yyyy"), now.ToString("MM"), now.ToString("dd"));
            Directory.CreateDirectory(datePath);
            var pdfPath = Path.Combine(datePath, fileName);
            var txtPath = Path.Combine(datePath, txtName);
            using (var stream = new FileStream(pdfPath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }
            await System.IO.File.WriteAllTextAsync(txtPath, faxNumber);
            // 寫入資料庫
            var record = new FaxRecord
            {
                FaxNumber = faxNumber,
                FaxTime = now,
                FileName = fileName,
                SourceSystem = "WebUI",
                SourceIP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
                InsurenceType = insurenceType,
                Department = department,
                Account = User.Identity?.Name,
                CreatedAt = now
            };
            _db.FaxRecords.Add(record);
            await _db.SaveChangesAsync();
            ViewBag.Message = "上傳成功";
            return View("Index");
        }
    }
}
