using NLog.Web;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Hangfire.SqlServer;
using Hangfire.SQLite;

var builder = WebApplication.CreateBuilder(args);

// 設定 NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<FaxMS.Models.DepartmentSyncService>();
builder.Services.AddScoped<FaxMS.Models.FaxCleanupService>();

// 註冊 Swagger（繁體中文文件）
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "傳真管理系統 API 文件",
        Version = "v1",
        Description = "本文件僅提供繁體中文說明。"
    });
});

// 註冊 Hangfire，依環境切換儲存方式
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHangfire(config =>
        config.UseSimpleAssemblyNameTypeSerializer()
              .UseRecommendedSerializerSettings()
              .UseSQLiteStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    builder.Services.AddHangfire(config =>
        config.UseSimpleAssemblyNameTypeSerializer()
              .UseRecommendedSerializerSettings()
              .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
}
builder.Services.AddHangfireServer();

// 註冊 DbContext，依環境切換 SQLite 或 SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<FaxMS.Models.FaxDbContext>(options =>
        options.UseSqlite(connectionString));
}
else
{
    builder.Services.AddDbContext<FaxMS.Models.FaxDbContext>(options =>
        options.UseSqlServer(connectionString));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// 啟用 Swagger（僅開發環境）
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "傳真管理系統 API 文件");
        options.DocumentTitle = "傳真管理系統 API 文件";
    });
}
app.UseStaticFiles();

// 啟用 Hangfire Dashboard（僅管理員可見，後續補權限控管）
app.UseHangfireDashboard("/hangfire");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var scope = app.Services.CreateScope();
var deptSync = scope.ServiceProvider.GetRequiredService<FaxMS.Models.DepartmentSyncService>();
RecurringJob.AddOrUpdate("SyncDepartments", () => deptSync.SyncDepartmentsAsync(), "0 1 * * *");

var faxCleanup = scope.ServiceProvider.GetRequiredService<FaxMS.Models.FaxCleanupService>();
RecurringJob.AddOrUpdate("CleanupOldFaxes", () => faxCleanup.CleanupOldFaxesAsync(), "0 2 * * *");

app.Run();
