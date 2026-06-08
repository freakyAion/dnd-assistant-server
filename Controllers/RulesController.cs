using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create([FromBody] Rule rule)
        {
            LogContext(nameof(Create));

            _context.Rules.Add(rule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBySlug), new { slug = rule.Slug }, rule);
        }
    }
}