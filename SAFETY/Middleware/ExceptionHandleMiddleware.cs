using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Middleware
{
    /// <summary>
    /// 全局例外處理
    /// </summary>
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExceptionHandleMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                // 寫Log
                var fileName = "Error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Log", fileName);
                //若Log目錄不存在則建立
                if (!Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "Log")))
                    Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "Log"));

                if (!File.Exists(filePath))
                {
                    //建立檔案
                    File.Create(filePath).Close();
                }

                // 寫入log 訂單紀錄
                using (var writer = File.AppendText(filePath))
                {
                    writer.WriteLine($"------------------{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}---------------------");
                    writer.Write(exception.StackTrace);
                    writer.WriteLine("");
                    writer.Write("[Error]" + exception.Message);
                    writer.WriteLine("");
                    writer.Write("[Inner Error]" + exception.InnerException?.Message);
                    writer.WriteLine("");
                    writer.WriteLine("-----------------------End---------------------------------------");
                }
                if (context.Request.Path.Value.Contains("api"))
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    await context.Response.WriteAsync(
                        $"{context.Response.StatusCode} Internal Server Error from the ExceptionHandle middleware."
                    );
                }
                else
                {
                    context.Response.Redirect("/Home/Error");
                }
            }
        }
    }
    public static class ExceptionHandleMiddlewareExtensions
    {
        /// <summary>在中介程序中全域處理例外</summary>
        /// <param name="builder">中介程序建構器</param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }
}
