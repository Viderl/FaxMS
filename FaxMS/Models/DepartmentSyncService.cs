using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace FaxMS.Models
{
    public class DepartmentSyncService
    {
        private readonly FaxDbContext _db;
        private readonly IHttpClientFactory _httpClientFactory;

        public DepartmentSyncService(FaxDbContext db, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _httpClientFactory = httpClientFactory;
        }

        public async Task SyncDepartmentsAsync()
        {
            var url = "https://api-test.tmnewa.com.tw/ebp/common/!Partner/TMNewa.Mis.Partner.Service.AIOuterService.svc/json/GetTMNewaDepartment";
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync(url);
            // TODO: 解析回傳資料並寫入資料庫
            // var departments = JsonSerializer.Deserialize<List<Department>>(response);
            // _db.Departments.UpdateRange(departments);
            // await _db.SaveChangesAsync();
        }
    }
}