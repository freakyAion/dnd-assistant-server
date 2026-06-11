using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CharacterOptionsController(MyDbContext context, ILogger<CharacterOptionsController> logger)
        : TemplateController(context, logger)
    {
        [AllowAnonymous]
        [HttpGet("species")]
        public async Task<IActionResult> GetSpecies()
        {
            LogContext(nameof(GetSpecies));

            var species = await _context.Species
                .Include(s => s.Traits)
                .OrderBy(s => s.Name)
                .Select(s => new
                {
                    s.ID,
                    s.Name,
                    s.Size,
                    s.BaseSpeed,
                    Description = s.Description,
                    Traits = s.Traits.Select(t => new { t.ID, t.Name, t.Description })
                })
                .ToListAsync();

            return Ok(species);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("species")]
        public async Task<IActionResult> CreateSpecies([FromBody] UpsertSpeciesDto dto)
        {
            LogContext(nameof(CreateSpecies));

            var descString = dto.Description?.ToString() ?? "{\"blocks\":[]}";
            using var descDoc = JsonDocument.Parse(descString);

            var species = new Species
            {
                Name = dto.Name,
                Size = dto.Size,
                BaseSpeed = dto.BaseSpeed,
                Description = JsonDocument.Parse(descDoc.RootElement.GetRawText())
            };

            _context.Species.Add(species);
            await _context.SaveChangesAsync();
            return Ok(species);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("species/{id:guid}")]
        public async Task<IActionResult> UpdateSpecies(Guid id, [FromBody] UpsertSpeciesDto dto)
        {
            LogContext(nameof(UpdateSpecies));

            var species = await _context.Species.FirstOrDefaultAsync(s => s.ID == id);
            if (species == null) return NotFound();

            var descString = dto.Description?.ToString() ?? "{\"blocks\":[]}";
            using var descDoc = JsonDocument.Parse(descString);

            species.Name = dto.Name;
            species.Size = dto.Size;
            species.BaseSpeed = dto.BaseSpeed;
            species.Description = JsonDocument.Parse(descDoc.RootElement.GetRawText());

            await _context.SaveChangesAsync();
            return Ok(species);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("species/{id:guid}")]
        public async Task<IActionResult> DeleteSpecies(Guid id)
        {
            LogContext(nameof(DeleteSpecies));

            var rows = await _context.Species.Where(s => s.ID == id).ExecuteDeleteAsync();
            if (rows == 0) return NotFound();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("backgrounds")]
        public async Task<IActionResult> GetBackgrounds()
        {
            LogContext(nameof(GetBackgrounds));

            var backgrounds = await _context.Backgrounds
                .Include(b => b.Features)
                .OrderBy(b => b.Name)
                .Select(b => new
                {
                    b.ID,
                    b.Name,
                    b.SkillProficiencies,
                    Description = b.Description,
                    Features = b.Features.Select(f => new { f.ID, f.Name, f.Description })
                })
                .ToListAsync();

            return Ok(backgrounds);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("backgrounds")]
        public async Task<IActionResult> CreateBackground([FromBody] UpsertBackgroundDto dto)
        {
            LogContext(nameof(CreateBackground));

            var descString = dto.Description?.ToString() ?? "{\"blocks\":[]}";
            using var descDoc = JsonDocument.Parse(descString);

            var background = new Background
            {
                Name = dto.Name,
                SkillProficiencies = dto.SkillProficiencies ?? new List<Skill>(),
                Description = JsonDocument.Parse(descDoc.RootElement.GetRawText())
            };

            _context.Backgrounds.Add(background);
            await _context.SaveChangesAsync();
            return Ok(background);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("backgrounds/{id:guid}")]
        public async Task<IActionResult> UpdateBackground(Guid id, [FromBody] UpsertBackgroundDto dto)
        {
            LogContext(nameof(UpdateBackground));

            var background = await _context.Backgrounds.FirstOrDefaultAsync(b => b.ID == id);
            if (background == null) return NotFound();

            var descString = dto.Description?.ToString() ?? "{\"blocks\":[]}";
            using var descDoc = JsonDocument.Parse(descString);

            background.Name = dto.Name;
            background.SkillProficiencies = dto.SkillProficiencies ?? new List<Skill>();
            background.Description = JsonDocument.Parse(descDoc.RootElement.GetRawText());

            await _context.SaveChangesAsync();
            return Ok(background);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("backgrounds/{id:guid}")]
        public async Task<IActionResult> DeleteBackground(Guid id)
        {
            LogContext(nameof(DeleteBackground));

            var rows = await _context.Backgrounds.Where(b => b.ID == id).ExecuteDeleteAsync();
            if (rows == 0) return NotFound();

            return NoContent();
        }
    }

    public class UpsertSpeciesDto
    {
        public string Name { get; set; } = string.Empty;
        public Species.CreatureSize Size { get; set; }
        public int BaseSpeed { get; set; }
        public System.Text.Json.Nodes.JsonNode? Description { get; set; }
    }

    public class UpsertBackgroundDto
    {
        public string Name { get; set; } = string.Empty;
        public List<Skill>? SkillProficiencies { get; set; }
        public System.Text.Json.Nodes.JsonNode? Description { get; set; }
    }
}