using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dnd_assistant.Controllers
{
    [Route("api/[controller]")]
    public class SpellsController(MyDbContext context, ILogger<SpellsController> logger)
        : TemplateController(context, logger)
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? level, [FromQuery] Spell.MagicSchool? school)
        {
            LogContext(nameof(GetAll));

            var query = _context.Spells.AsQueryable();

            if (level.HasValue)
                query = query.Where(s => s.Level == level.Value);

            if (school.HasValue)
                query = query.Where(s => s.School == school.Value);

            var spells = await query.OrderBy(s => s.Level).ThenBy(s => s.Name).ToListAsync();
            return Ok(spells);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            LogContext(nameof(GetById));

            var spell = await _context.Spells.FirstOrDefaultAsync(s => s.ID == id);
            if (spell == null) return NotFound();

            return Ok(spell);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create([FromBody] Spell spell)
        {
            LogContext(nameof(Create));

            _context.Spells.Add(spell);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = spell.ID }, spell);
        }
    }
}