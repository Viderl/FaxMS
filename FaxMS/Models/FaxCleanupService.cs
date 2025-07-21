using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaxMS.Models
{
    public class FaxCleanupService
    {
        private readonly FaxDbContext _db;

        public FaxCleanupService(FaxDbContext db)
        {
            _db = db;
        }

        public async Task CleanupOldFaxesAsync()
        {
            var threshold = DateTime.Now.AddYears(-5);
            var oldFaxes = _db.Faxes.Where(f => f.ReceivedAt < threshold).ToList();

            foreach (var fax in oldFaxes)
            {
                if (!string.IsNullOrEmpty(fax.FilePath) && File.Exists(fax.FilePath))
                    File.Delete(fax.FilePath);
                if (!string.IsNullOrEmpty(fax.TxtPath) && File.Exists(fax.TxtPath))
                    File.Delete(fax.TxtPath);
                if (!string.IsNullOrEmpty(fax.CompletedFilePath) && File.Exists(fax.CompletedFilePath))
                    File.Delete(fax.CompletedFilePath);
                if (!string.IsNullOrEmpty(fax.CompletedTxtPath) && File.Exists(fax.CompletedTxtPath))
                    File.Delete(fax.CompletedTxtPath);
                _db.Faxes.Remove(fax);
            }
            await _db.SaveChangesAsync();
        }
    }
}