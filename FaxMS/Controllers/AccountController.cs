using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.Protocols;
using System.Net;
using FaxMS.Models;
using FaxMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FaxMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly FaxMSDbContext _db;
        private readonly IConfiguration _config;
        public AccountController(FaxMSDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string account, string password, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "帳號與密碼必填");
                return View();
            }
            // LDAP 驗證
            if (!LdapAuthenticate(account, password))
            {
                ModelState.AddModelError("", "AD 驗證失敗");
                return View();
            }
            // 取得管理員身分
            var isAdmin = await _db.Admins.AnyAsync(a => a.Account == account && a.IsActive);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account),
                new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return Redirect(returnUrl ?? "/");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private bool LdapAuthenticate(string account, string password)
        {
            try
            {
                var ldapServer = _config["Ldap:Server"] ?? "your-ldap-server";
                var ldapDomain = _config["Ldap:Domain"] ?? "your-domain";
                using var ldap = new LdapConnection(ldapServer);
                var credential = new NetworkCredential($"{ldapDomain}\\{account}", password);
                ldap.AuthType = AuthType.Negotiate;
                ldap.Bind(credential);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
