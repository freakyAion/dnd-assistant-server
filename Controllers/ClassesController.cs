using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateClass([FromBody] UpsertClassDto dto)
        {
            LogContext(nameof(CreateClass));

            var jsonString = dto.Description?.ToString() ?? "{\"blocks\":[]}";
            using var doc = JsonDocument.Parse(jsonString);

            var @class = new Class
            {
                Name = dto.Name,
                HitDieSides = dto.HitDieSides,
                // Parse the raw text directly to create a standalone instance
                Description = JsonDocument.Parse(doc.RootElement.GetRawText()),
                SavingThrows = dto.SavingThrows
            };

            _context.Classes.Add(@class);
            await _context.SaveChangesAsync();
            return Ok(@class);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateClass(Guid id, [FromBody] UpsertClassDto dto)
        {
            LogContext(nameof(UpdateClass));

            var @class = await _context.Classes.FirstOrDefaultAsync(c => c.ID == id);
            if (@class == null) return NotFound();

            var jsonString = dto.Description?.ToString() ?? "{\"blocks\":[]}";
            using var doc = JsonDocument.Parse(jsonString);

            @class.Name = dto.Name;
            @class.HitDieSides = dto.HitDieSides;
            @class.Description = JsonDocument.Parse(doc.RootElement.GetRawText());
            @class.SavingThrows = dto.SavingThrows;

            await _context.SaveChangesAsync();
            return Ok(@class);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteClass(Guid id)
        {
            LogContext(nameof(DeleteClass));

            var rowsAffected = await _context.Classes.Where(c => c.ID == id).ExecuteDeleteAsync();
            if (rowsAffected == 0) return NotFound();

            return NoContent();
        }
    }

    public class UpsertClassDto
    {
        public string Name { get; set; } = string.Empty;
        public int HitDieSides { get; set; }
        public System.Text.Json.Nodes.JsonNode? Description { get; set; }
        public Class.SavingThrowFlags SavingThrows { get; set; }
    }
}