using dnd_assistant.DB;
using Microsoft.AspNetCore.Mvc;

namespace dnd_assistant.Controllers
{
    [ApiController]
    [Route("api/serverstatus")]
    public class ServerStatusController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger) : TemplateController(context, contextAccessor, logger)
    {
        [HttpGet]
        public IActionResult Get()
        {
            LogContext("Ping!");
            var response = new { response = "Hello World!" };
            return Ok(response);
        }
    }
}
