using dnd_assistant.DB;
using Microsoft.AspNetCore.Mvc;

namespace dnd_assistant.Controllers
{
    [ApiController]
    public abstract class TemplateController(MyDbContext context, ILogger<TemplateController> logger) : ControllerBase
    {
        protected readonly MyDbContext _context = context;
        protected readonly ILogger<TemplateController> _logger = logger;

        [NonAction]
        public void LogContext(string requestName)
        {
            // TODO: Add logging via a .txt file if necessary, 
            // though standard ILogger handles file outputs cleanly via providers like Serilog.

            var request = HttpContext.Request;
            var connection = HttpContext.Connection;

            var method = request.Method;
            var headers = request.Headers;
            var requestPath = request.Path;
            var clientIp = connection.RemoteIpAddress?.ToString();

            var contentType = headers["Content-Type"];
            var userAgent = headers["User-Agent"];

            _logger.LogInformation(
                "{Timestamp} - {RequestName}\n" +
                "Method: {Method}, Content-Type: {ContentType}\n" +
                "From: {Path}, {Ip}, {UserAgent}\n",
                DateTime.UtcNow, requestName, method, contentType, requestPath, clientIp, userAgent
            );
        }
    }
}