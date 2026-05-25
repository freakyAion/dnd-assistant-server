using dnd_assistant.DB;
using dnd_assistant.Models;

namespace dnd_assistant.Controllers
{
    public class WorldsController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<World>(context, contextAccessor, logger)
    { }

    public class WorldAccessController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<WorldAccess>(context, contextAccessor, logger)
    { }

    public class CampaignsController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<Campaign>(context, contextAccessor, logger)
    { }

    public class CampaignAccessController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<CampaignAccess>(context, contextAccessor, logger)
    { }

    public class LocationsController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<Location>(context, contextAccessor, logger)
    { }

    public class EventsController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<Event>(context, contextAccessor, logger)
    { }

    public class SessionsController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<Session>(context, contextAccessor, logger)
    { }

    public class OrganisationsController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<Organisation>(context, contextAccessor, logger)
    { }

    public class NPCsController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<NPC>(context, contextAccessor, logger)
    { }
}