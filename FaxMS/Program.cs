using Microsoft.EntityFrameworkCore;
using FaxMS.Models;

var builder = WebApplication.CreateBuilder(args);

// 註冊DbContext（預設SQLite）
builder.Services.AddDbContext<FaxMS.Data.FaxMSDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 註冊Swagger（繁體中文）
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "傳真管理系統 API",
        Version = "v1",
        Description = "傳真管理系統 API 文件",
    });
    c.DocumentFilter<TaiwanChineseSwaggerDocumentFilter>();
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 啟用Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "傳真管理系統 API v1");
    c.DocumentTitle = "傳真管理系統 API 文件";
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
