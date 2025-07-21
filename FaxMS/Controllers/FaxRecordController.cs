using FaxMS.Data;
using FaxMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaxMS.Controllers
{
    [Authorize]
    public class FaxRecordController : Controller
    {
        private readonly FaxMSDbContext _db;
        public FaxRecordController(FaxMSDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? faxNumber, DateTime? faxTimeFrom, DateTime? faxTimeTo)
        {
            var user = User.Identity?.Name;
            var isAdmin = User.IsInRole("Admin");
            var query = _db.FaxRecords.AsQueryable();
            if (!string.IsNullOrWhiteSpace(faxNumber))
                query = query.Where(f => f.FaxNumber.Contains(faxNumber));
            if (faxTimeFrom.HasValue)
                query = query.Where(f => f.FaxTime >= faxTimeFrom);
            if (faxTimeTo.HasValue)
                query = query.Where(f => f.FaxTime <= faxTimeTo);
            if (!isAdmin)
                query = query.Where(f => f.Account == user);
            var list = await query.OrderByDescending(f => f.FaxTime).ToListAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            var record = await _db.FaxRecords.FindAsync(id);
            if (record == null)
                return NotFound();
            var isAdmin = User.IsInRole("Admin");
            var user = User.Identity?.Name;
            if (!isAdmin && record.Account != user)
                return Forbid();
            // 檔案路徑
            var baseDir = (HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration)?["Fax:BaseDir"] ?? Path.Combine(AppContext.BaseDirectory, "FaxFiles");
            var dt = record.FaxTime;
            var datePath = Path.Combine(baseDir, dt.ToString("yyyy"), dt.ToString("MM"), dt.ToString("dd"));
            var pdfPath = Path.Combine(datePath, record.FileName);
            if (!System.IO.File.Exists(pdfPath))
                return NotFound("檔案不存在");
            // 寫入調閱紀錄
            _db.FaxRecordAccesses.Add(new FaxRecordAccess
            {
                FaxRecordId = record.Id,
                Account = user ?? "",
                AccessedAt = DateTime.Now
            });
            await _db.SaveChangesAsync();
            var fileBytes = await System.IO.File.ReadAllBytesAsync(pdfPath);
            return File(fileBytes, "application/pdf", record.FileName);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _db.FaxRecords.FindAsync(id);
            if (record == null)
                return NotFound();
            // 檔案路徑
            var baseDir = (HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration)?["Fax:BaseDir"] ?? Path.Combine(AppContext.BaseDirectory, "FaxFiles");
            var dt = record.FaxTime;
            var datePath = Path.Combine(baseDir, dt.ToString("yyyy"), dt.ToString("MM"), dt.ToString("dd"));
            var pdfPath = Path.Combine(datePath, record.FileName);
            var txtPath = Path.ChangeExtension(pdfPath, ".txt");
            if (System.IO.File.Exists(pdfPath)) System.IO.File.Delete(pdfPath);
            if (System.IO.File.Exists(txtPath)) System.IO.File.Delete(txtPath);
            _db.FaxRecords.Remove(record);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
