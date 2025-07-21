using Microsoft.AspNetCore.Mvc;
using FaxMS.Models;

namespace FaxMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string account, string password)
        {
            // 參數可移至 appsettings
            var ldapServer = _config["Ldap:Server"] ?? "";
            var ldapPort = int.TryParse(_config["Ldap:Port"], out var port) ? port : 389;
            var ldapDomain = _config["Ldap:Domain"] ?? "";

            var ldap = new LdapAuthService(ldapServer, ldapPort, ldapDomain);
            if (ldap.ValidateUser(account, password))
            {
                // 登入成功，後續可設定 session 或 cookie
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "AD 帳號或密碼錯誤";
            return View();
        }
    }
}