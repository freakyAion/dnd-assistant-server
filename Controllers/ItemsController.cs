using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dnd_assistant.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController(MyDbContext context, ILogger<ItemsController> logger)
        : TemplateController(context, logger)
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Item.ItemType? type)
        {
            LogContext(nameof(GetAll));

            var query = _context.Items.AsQueryable();

            if (type.HasValue)
                query = query.Where(i => i.Type == type.Value);

            var items = await query.OrderBy(i => i.Name).ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            LogContext(nameof(GetById));

            var item = await _context.Items.FirstOrDefaultAsync(i => i.ID == id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create([FromBody] Item item)
        {
            LogContext(nameof(Create));

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.ID }, item);
        }
    }
}