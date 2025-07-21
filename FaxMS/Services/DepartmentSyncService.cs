using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace FaxMS.Services
{
    public class DepartmentSyncService : BackgroundService
    {
        private readonly ILogger<DepartmentSyncService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        public DepartmentSyncService(ILogger<DepartmentSyncService> logger, IServiceProvider serviceProvider, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SyncDepartmentsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "部門同步失敗");
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // 每24小時執行一次
            }
        }

        private async Task SyncDepartmentsAsync()
        {
            var url = _config["DepartmentApi:Url"] ?? "https://api-test.tmnewa.com.tw/ebp/common/!Partner/TMNewa.Mis.Partner.Service.AIOuterService.svc/json/GetTMNewaDepartment";
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            // TODO: 解析json並寫入資料庫
            _logger.LogInformation("部門同步完成");
        }
    }
}
