using Microsoft.AspNetCore.Mvc;
using FaxMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace FaxMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaxApiController : ControllerBase
    {
        private readonly FaxDbContext _db;
        private readonly ILogger<FaxApiController> _logger;
        private readonly IConfiguration _config;

        public FaxApiController(FaxDbContext db, ILogger<FaxApiController> logger, IConfiguration config)
        {
            _db = db;
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// 接收傳真 API
        /// </summary>
        [HttpPost("Receive")]
        [RequestSizeLimit(3 * 1024 * 1024)]
        public async Task<IActionResult> Receive([FromForm] FaxReceiveRequest req)
        {
            // 來源IP限制
            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var sourceSystem = _db.SourceSystems.Include(s => s.IPs).FirstOrDefault(s => s.Name == req.SourceSystem);
            if (sourceSystem == null || !sourceSystem.IPs.Any(ip => ip.IP == remoteIp))
                return StatusCode(403, "來源IP不允許");

            // 險種驗證
            if (!Enum.TryParse(typeof(EnumInsurenceType), req.InsuranceType, out _))
                return BadRequest("險種錯誤");

            // 部門驗證（應同步外部API，這裡略）
            // if (!CheckDepartment(req.Department)) return BadRequest("部門錯誤");

            // PDF檔案驗證
            if (req.PdfFile == null || req.PdfFile.Length == 0)
                return BadRequest("PDF檔案必填");
            if (req.PdfFile.Length > 3 * 1024 * 1024)
                return BadRequest("PDF檔案過大");

            // 儲存檔案
            var now = DateTime.Now;
            var folder = Path.Combine(_config["Fax:Root"], now.ToString("yyyy"), now.ToString("MM"), now.ToString("dd"));
            Directory.CreateDirectory(folder);
            var fileName = now.ToString("yyyyMMddHHmmssffff") + ".pdf";
            var filePath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await req.PdfFile.CopyToAsync(stream);
            }
            // 儲存號碼txt
            var txtPath = Path.Combine(folder, now.ToString("yyyyMMddHHmmssffff") + ".txt");
            await System.IO.File.WriteAllTextAsync(txtPath, req.FaxNumber);

            // 寫入資料庫
            var fax = new Fax
            {
                FaxNumber = req.FaxNumber,
                ReceivedAt = now,
                SourceSystem = req.SourceSystem,
                Department = req.Department,
                InsuranceType = req.InsuranceType,
                FilePath = filePath,
                TxtPath = txtPath,
                SenderAccount = req.SenderAccount
            };
            _db.Faxes.Add(fax);
            await _db.SaveChangesAsync();

            _logger.LogInformation("收到傳真，帳號:{Account}, IP:{IP}", req.SenderAccount, remoteIp);

            return Ok("傳真接收成功");
        }
    }

    public class FaxReceiveRequest
    {
        [FromForm(Name = "pdfFile")]
        public Microsoft.AspNetCore.Http.IFormFile PdfFile { get; set; }
        [FromForm(Name = "faxNumber")]
        public string FaxNumber { get; set; }
        [FromForm(Name = "sourceSystem")]
        public string SourceSystem { get; set; }
        [FromForm(Name = "insuranceType")]
        public string InsuranceType { get; set; }
        [FromForm(Name = "department")]
        public string Department { get; set; }
        [FromForm(Name = "senderAccount")]
        public string SenderAccount { get; set; }
    }

    public enum EnumInsurenceType
    {
        A, // 傷害險
        B, // 商火
        C, // 車險
        F, // 住火
        H, // 健康險
        M, // 水險
        O  // 新種險
    }
}