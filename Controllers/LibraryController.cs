using dnd_assistant.DB;
using dnd_assistant.Models;

namespace dnd_assistant.Controllers
{
    public class SpellsController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<Spell>(context, contextAccessor, logger)
    { }

    public class ItemsController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<Item>(context, contextAccessor, logger)
    { }

    public class SpeciesController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<Species>(context, contextAccessor, logger)
    { }

    public class BackgroundsController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : GenericCrudController<Background>(context, contextAccessor, logger)
    { }
}