using dnd_assistant.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dnd_assistant.Controllers
{
    [Route("api/[controller]")]
    public class CharacterOptionsController(MyDbContext context, ILogger<CharacterOptionsController> logger)
        : TemplateController(context, logger)
    {
        [HttpGet("species")]
        public async Task<IActionResult> GetSpecies()
        {
            LogContext(nameof(GetSpecies));

            var species = await _context.Species
                .Include(s => s.Traits)
                .OrderBy(s => s.Name)
                .ToListAsync();

            return Ok(species);
        }

        [HttpGet("backgrounds")]
        public async Task<IActionResult> GetBackgrounds()
        {
            LogContext(nameof(GetBackgrounds));

            var backgrounds = await _context.Backgrounds
                .Include(b => b.Features)
                .OrderBy(b => b.Name)
                .ToListAsync();

            return Ok(backgrounds);
        }
    }
}