using FaxMS.Data;
using Microsoft.EntityFrameworkCore;

namespace FaxMS.Jobs
{
    public class SystemJobs
    {
        private readonly FaxMSDbContext _db;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public SystemJobs(FaxMSDbContext db, IConfiguration config, IWebHostEnvironment env)
        {
            _db = db;
            _config = config;
            _env = env;
        }

        // 清理超過5年傳真紀錄與檔案
        public async Task CleanOldFaxAsync()
        {
            var threshold = DateTime.Now.AddYears(-5);
            var oldFaxes = await _db.FaxRecords.Where(f => f.FaxTime < threshold).ToListAsync();
            foreach (var fax in oldFaxes)
            {
                var dt = fax.FaxTime;
                var baseDir = _config["Fax:BaseDir"] ?? Path.Combine(_env.ContentRootPath, "FaxFiles");
                var datePath = Path.Combine(baseDir, dt.ToString("yyyy"), dt.ToString("MM"), dt.ToString("dd"));
                var pdfPath = Path.Combine(datePath, fax.FileName);
                var txtPath = Path.ChangeExtension(pdfPath, ".txt");
                if (File.Exists(pdfPath)) File.Delete(pdfPath);
                if (File.Exists(txtPath)) File.Delete(txtPath);
                _db.FaxRecords.Remove(fax);
            }
            await _db.SaveChangesAsync();
        }

        // 同步部門清單（僅示意，實際應呼叫API並更新DB）
        public async Task SyncDepartmentsAsync()
        {
            // TODO: 呼叫API並更新部門資料表
            await Task.CompletedTask;
        }
    }
}
