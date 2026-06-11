using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace dnd_assistant.Controllers
{
    [Route("api/[controller]")]
    public class RulesController(MyDbContext context, ILogger<RulesController> logger)
        : TemplateController(context, logger)
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Rule.RuleCategory? category)
        {
            LogContext(nameof(GetAll));

            var query = _context.Rules.AsQueryable();

            if (category.HasValue)
                query = query.Where(r => r.Category == category.Value);

            var rules = await query.OrderBy(r => r.Category).ThenBy(r => r.Title).ToListAsync();
            return Ok(rules);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            LogContext(nameof(GetBySlug));

            var rule = await _context.Rules.FirstOrDefaultAsync(r => r.Slug.ToLower() == slug.ToLower());
            if (rule == null) return NotFound();

            return Ok(rule);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] UpsertRuleDto dto)
        {
            LogContext(nameof(Create));

            if (await _context.Rules.AnyAsync(r => r.Slug.ToLower() == dto.Slug.ToLower()))
            {
                return BadRequest("Правило с таким уникальным Slug уже существует.");
            }

            var jsonString = dto.Content?.ToString() ?? "{\"blocks\":[]}";
            using var doc = JsonDocument.Parse(jsonString);

            var rule = new Rule
            {
                Title = dto.Title,
                Slug = dto.Slug.ToLower().Trim(),
                Category = dto.Category,
                Content = JsonDocument.Parse(doc.RootElement.GetRawText())
            };

            _context.Rules.Add(rule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBySlug), new { slug = rule.Slug }, rule);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpsertRuleDto dto)
        {
            LogContext(nameof(Update));

            var rule = await _context.Rules.FirstOrDefaultAsync(r => r.ID == id);
            if (rule == null) return NotFound();

            if (rule.Slug.ToLower() != dto.Slug.ToLower().Trim() &&
                await _context.Rules.AnyAsync(r => r.Slug.ToLower() == dto.Slug.ToLower().Trim()))
            {
                return BadRequest("Правило с таким уникальным Slug уже существует.");
            }

            var jsonString = dto.Content?.ToString() ?? "{\"blocks\":[]}";
            using var doc = JsonDocument.Parse(jsonString);

            rule.Title = dto.Title;
            rule.Slug = dto.Slug.ToLower().Trim();
            rule.Category = dto.Category;
            rule.Content = JsonDocument.Parse(doc.RootElement.GetRawText());

            await _context.SaveChangesAsync();
            return Ok(rule);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            LogContext(nameof(Delete));

            var rowsAffected = await _context.Rules.Where(r => r.ID == id).ExecuteDeleteAsync();
            if (rowsAffected == 0) return NotFound();

            return NoContent();
        }
    }

    public class UpsertRuleDto
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public Rule.RuleCategory Category { get; set; }
        public System.Text.Json.Nodes.JsonNode? Content { get; set; }
    }
}