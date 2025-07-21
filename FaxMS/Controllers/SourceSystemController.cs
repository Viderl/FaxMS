using FaxMS.Data;
using FaxMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaxMS.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class SourceSystemController : Controller
    {
        private readonly FaxMSDbContext _db;
        public SourceSystemController(FaxMSDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var systems = await _db.SourceSystems.Include(s => s.IPs).ToListAsync();
            return View(systems);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("", "來源系統名稱必填");
                return View();
            }
            if (await _db.SourceSystems.AnyAsync(s => s.Name == name))
            {
                ModelState.AddModelError("", "來源系統已存在");
                return View();
            }
            var sys = new SourceSystem
            {
                Name = name,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity?.Name ?? "system"
            };
            _db.SourceSystems.Add(sys);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var sys = await _db.SourceSystems.Include(s => s.IPs).FirstOrDefaultAsync(s => s.Id == id);
            if (sys == null) return NotFound();
            return View(sys);
        }

        [HttpPost]
        public async Task<IActionResult> AddIP(int id, string ip)
        {
            var sys = await _db.SourceSystems.FindAsync(id);
            if (sys == null) return NotFound();
            if (string.IsNullOrWhiteSpace(ip))
            {
                TempData["Error"] = "IP 必填";
                return RedirectToAction("Edit", new { id });
            }
            if (await _db.SourceSystemIPs.AnyAsync(x => x.IP == ip && x.SourceSystemId == id))
            {
                TempData["Error"] = "IP 已存在";
                return RedirectToAction("Edit", new { id });
            }
            var ipEntity = new SourceSystemIP
            {
                IP = ip,
                SourceSystemId = id,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity?.Name ?? "system"
            };
            _db.SourceSystemIPs.Add(ipEntity);
            await _db.SaveChangesAsync();
            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIP(int id, int ipId)
        {
            var ip = await _db.SourceSystemIPs.FindAsync(ipId);
            if (ip != null && ip.SourceSystemId == id)
            {
                _db.SourceSystemIPs.Remove(ip);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Edit", new { id });
        }
    }
}
