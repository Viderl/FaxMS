using FaxMS.Data;
using FaxMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaxMS.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly FaxMSDbContext _db;
        public AdminController(FaxMSDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var admins = await _db.Admins.ToListAsync();
            return View(admins);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                ModelState.AddModelError("", "帳號必填");
                return View();
            }
            if (await _db.Admins.AnyAsync(a => a.Account == account))
            {
                ModelState.AddModelError("", "帳號已存在");
                return View();
            }
            var admin = new Admin
            {
                Account = account,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity?.Name ?? "system",
                IsActive = true
            };
            _db.Admins.Add(admin);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var admin = await _db.Admins.FindAsync(id);
            if (admin != null)
            {
                admin.IsActive = !admin.IsActive;
                admin.UpdatedAt = DateTime.Now;
                admin.UpdatedBy = User.Identity?.Name ?? "system";
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
