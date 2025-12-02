using dnd_assistant.DB;
using Microsoft.AspNetCore.Mvc;

namespace dnd_assistant.Controllers
{
    public class TemplateController : ControllerBase
    {
        protected readonly IHttpContextAccessor contextAccessor;
        protected readonly ILogger<TemplateController> logger;
        protected readonly MyDbContext context;

        [NonAction]
        public void LogContext(string requestName)
        {
            // TODO: Add logging via a .txt file

            var method = contextAccessor.HttpContext?.Request?.Method;
            var headers = contextAccessor.HttpContext?.Request?.Headers;
            var requestPath = contextAccessor.HttpContext?.Request?.Path;
            var clientIp = contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            var contentType = headers?["Content-Type"];
            var userAgent = headers?["User-Agent"];

            logger.LogInformation(
                $"{DateTime.Now} - {requestName}\n" +
                $"Method: {method}, Content-Type: {contentType}\n" +
                $"From: {requestPath}, {clientIp}, {userAgent}\n"
            );

        }

        public TemplateController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        {
            this.context = context;
            this.contextAccessor = contextAccessor;
            this.logger = logger;
        }
    }
}
