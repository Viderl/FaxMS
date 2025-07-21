

using FaxMS.Data;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Hangfire;
using Hangfire.SQLite;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddDbContext<FaxMSDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddAuthentication("Cookies")
        .AddCookie("Cookies", options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
        });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    });
    builder.Services.AddScoped<FaxMS.Jobs.SystemJobs>();
    builder.Services.AddHostedService<FaxMS.Services.DepartmentSyncService>();
    builder.Services.AddHttpClient();
    builder.Services.AddControllersWithViews();
    builder.Services.AddHangfire(x => x.UseSQLiteStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddHangfireServer();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "傳真管理系統 API",
            Version = "v1",
            Description = "Fax Management System API 文件 (繁體中文)"
        });
    });
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // 註冊 Hangfire 排程
    using (var scope = app.Services.CreateScope())
    {
        var jobs = scope.ServiceProvider.GetRequiredService<FaxMS.Jobs.SystemJobs>();
        Hangfire.RecurringJob.AddOrUpdate("CleanOldFax", () => jobs.CleanOldFaxAsync(), Cron.Daily);
        Hangfire.RecurringJob.AddOrUpdate("SyncDepartments", () => jobs.SyncDepartmentsAsync(), "0 1 * * *");
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    // 啟用 Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "傳真管理系統 API v1");
        c.DocumentTitle = "傳真管理系統 API 文件";
    });

    // 啟用 Hangfire Dashboard
    app.UseHangfireDashboard("/hangfire");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "應用程式啟動失敗");
    throw;
}
