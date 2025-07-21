using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FaxMS.Models
{
    /// <summary>
    /// Swagger文件繁體中文過濾器（預留，暫無實作）
    /// </summary>
    public class TaiwanChineseSwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // 可於此處進行繁體中文描述處理
        }
    }
}
