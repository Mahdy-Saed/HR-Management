using System.Diagnostics;
using System.Text;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
namespace HR_Carrer.Middleware
{

    public class DetailedRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public DetailedRequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // ===== قراءة الـ Body للـ Request =====
            context.Request.EnableBuffering(); // يسمح بقراءة الـ body أكثر من مرة
            string requestBody = "";
            if (context.Request.ContentLength > 0)
            {
                using (var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0; // reset position
                }
            }

            // ===== استخرج User ID إذا موجود =====
            string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";

            // ===== حفظ Response Body مؤقتاً =====
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            stopwatch.Stop();

            // ===== قراءة Response Body =====
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // ===== الطباعة على Console =====
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine($"Time: {DateTime.Now}");
            Console.WriteLine($"User: {userId}");
            Console.WriteLine($"Method: {context.Request.Method}, Path: {context.Request.Path}");
            if (!string.IsNullOrEmpty(requestBody))
                Console.WriteLine($"Request Body: {requestBody}");
            Console.WriteLine($"Status Code: {context.Response.StatusCode}, Duration: {stopwatch.ElapsedMilliseconds}ms");
            if (!string.IsNullOrEmpty(responseText))
                Console.WriteLine($"Response Body: {responseText}");
            Console.WriteLine("---------------------------------------------------");

            // إعادة الـ Response الأصلي
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

}
