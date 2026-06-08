using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dnd_assistant.Controllers
{
    [Route("api/[controller]")]
    public class ClassesController(MyDbContext context, ILogger<ClassesController> logger)
        : TemplateController(context, logger)
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            LogContext(nameof(GetAll));

            var classes = await _context.Classes.OrderBy(c => c.Name).ToListAsync();
            return Ok(classes);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            LogContext(nameof(GetById));

            var classData = await _context.Classes
                .Include(c => c.Progressions.OrderBy(p => p.Level))
                    .ThenInclude(p => p.Features)
                .Include(c => c.Spells)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (classData == null) return NotFound();

            return Ok(classData);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create([FromBody] Class classEntity)
        {
            LogContext(nameof(Create));

            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = classEntity.ID }, classEntity);
        }
    }
}