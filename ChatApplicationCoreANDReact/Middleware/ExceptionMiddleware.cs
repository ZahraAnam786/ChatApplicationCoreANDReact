using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatApplicationCoreANDReact.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continue request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                message = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred.",
                details = _env.IsDevelopment() ? ex.StackTrace : null
            };

            var path = "ErrorLog";
            if (context.Request.Path != null && context.Request.Path != "")
            {
                path = context.Request.Path;
                path = path.Replace("/", "_");
            }

            WriteLog(path, response.message, response.details);

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }


        private void WriteLog(string Request, string Message, string Details)
        {
            string folderPath = Path.Combine(_env.ContentRootPath, "Files", "Temp", "AppLogs");

            //   var folderPath = HttpContext.Current.Server.MapPath(@"~\Files\Temp\ADMSLogs\");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, Request + ".txt");
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath).Dispose();
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine + "=================================" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + " " + Message + "=================================");
            sb.Append(Environment.NewLine + Details);
            sb.Append(Environment.NewLine + "===================================================================================================================");
            System.IO.File.AppendAllText(filePath, sb.ToString());
            sb.Clear();
        }
    }

}


