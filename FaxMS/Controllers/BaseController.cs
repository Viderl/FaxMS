using Microsoft.AspNetCore.Mvc;
using FaxMS.Models;
using System.Linq;

namespace FaxMS.Controllers
{
    public class BaseController : Controller
    {
        protected readonly FaxDbContext _db;

        public BaseController(FaxDbContext db)
        {
            _db = db;
        }

        protected bool IsAdmin
        {
            get
            {
                var account = User.Identity?.Name ?? "";
                return _db.Admins.Any(a => a.Account == account && a.IsActive);
            }
        }
    }
}