using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace dnd_assistant.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SpellsController(MyDbContext context, ILogger<SpellsController> logger)
        : TemplateController(context, logger)
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllSpells()
        {
            var spells = await _context.Spells.OrderBy(s => s.Name).ToListAsync();
            return Ok(spells);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSpell([FromBody] UpsertSpellDto dto)
        {
            LogContext(nameof(CreateSpell));

            var jsonString = dto.Description?.ToString() ?? "{\"blocks\":[]}";
            using var doc = JsonDocument.Parse(jsonString);

            var spell = new Spell
            {
                Name = dto.Name,
                Level = dto.Level,
                School = dto.School,
                // RootElement.Clone() extracts a standalone, safe deep copy of the node hierarchy
                Description = JsonDocument.Parse(doc.RootElement.GetRawText()),
                HigherLevelDescription = dto.HigherLevelDescription,
                CastingTimeValue = dto.CastingTimeValue,
                CastingTimeType = dto.CastingTimeType,
                CastingTimeCondition = dto.CastingTimeCondition,
                RangeUnits = dto.RangeUnits,
                RangeValue = dto.RangeValue,
                AoeType = dto.AoeType,
                AoeValue = dto.AoeValue,
                Components = dto.Components,
                MaterialComponents = dto.MaterialComponents,
                MaterialCostGp = dto.MaterialCostGp,
                MaterialConsumed = dto.MaterialConsumed,
                DurationUnits = dto.DurationUnits,
                DurationValue = dto.DurationValue,
                RequiresConcentration = dto.RequiresConcentration
            };

            _context.Spells.Add(spell);
            await _context.SaveChangesAsync();
            return Ok(spell);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSpell(Guid id, [FromBody] UpsertSpellDto dto)
        {
            LogContext(nameof(UpdateSpell));

            var spell = await _context.Spells.FirstOrDefaultAsync(s => s.ID == id);
            if (spell == null) return NotFound();

            var jsonString = dto.Description?.ToString() ?? "{\"blocks\":[]}";
            using var doc = JsonDocument.Parse(jsonString);

            spell.Name = dto.Name;
            spell.Level = dto.Level;
            spell.School = dto.School;
            spell.Description = JsonDocument.Parse(doc.RootElement.GetRawText());
            spell.HigherLevelDescription = dto.HigherLevelDescription;
            spell.CastingTimeValue = dto.CastingTimeValue;
            spell.CastingTimeType = dto.CastingTimeType;
            spell.CastingTimeCondition = dto.CastingTimeCondition;
            spell.RangeUnits = dto.RangeUnits;
            spell.RangeValue = dto.RangeValue;
            spell.AoeType = dto.AoeType;
            spell.AoeValue = dto.AoeValue;
            spell.Components = dto.Components;
            spell.MaterialComponents = dto.MaterialComponents;
            spell.MaterialCostGp = dto.MaterialCostGp;
            spell.MaterialConsumed = dto.MaterialConsumed;
            spell.DurationUnits = dto.DurationUnits;
            spell.DurationValue = dto.DurationValue;
            spell.RequiresConcentration = dto.RequiresConcentration;

            await _context.SaveChangesAsync();
            return Ok(spell);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSpell(Guid id)
        {
            LogContext(nameof(DeleteSpell));

            var rowsAffected = await _context.Spells.Where(s => s.ID == id).ExecuteDeleteAsync();
            if (rowsAffected == 0) return NotFound();

            return NoContent();
        }

        public class UpsertSpellDto
        {
            public string Name { get; set; } = string.Empty;
            public int Level { get; set; }
            public Spell.MagicSchool School { get; set; }
            public System.Text.Json.Nodes.JsonNode? Description { get; set; }
            public string? HigherLevelDescription { get; set; }
            public int CastingTimeValue { get; set; } = 1;
            public Spell.CastingTimeUnit CastingTimeType { get; set; }
            public string? CastingTimeCondition { get; set; }
            public Spell.RangeUnit RangeUnits { get; set; }
            public int? RangeValue { get; set; }
            public dnd_assistant.Shared.Enums.AreaOfEffectShape? AoeType { get; set; }
            public int? AoeValue { get; set; }
            public Spell.ComponentFlags Components { get; set; }
            public string? MaterialComponents { get; set; }
            public int MaterialCostGp { get; set; }
            public bool MaterialConsumed { get; set; }
            public Spell.DurationUnit DurationUnits { get; set; }
            public int? DurationValue { get; set; }
            public bool RequiresConcentration { get; set; }
        }
    }
}