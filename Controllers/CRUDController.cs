using dnd_assistant.Abstract;
using dnd_assistant.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dnd_assistant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericCrudController<T>(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger)
        : TemplateController(context, contextAccessor, logger) where T : GuidEntity
    {
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            LogContext($"Get All {typeof(T).Name}s");
            var items = await _dbSet.ToListAsync();
            return Ok(new { response = "Success", data = items });
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(Guid id)
        {
            LogContext($"Get {typeof(T).Name} by ID");
            var item = await _dbSet.FirstOrDefaultAsync(e => e.ID == id);

            if (item == null) return NotFound(new { response = "Not Found" });

            return Ok(new { response = "Success", data = item });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] T entity)
        {
            LogContext($"Create {typeof(T).Name}");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.ID }, new { response = "Success", data = entity });
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(Guid id, [FromBody] T entity)
        {
            LogContext($"Update {typeof(T).Name}");
            if (id != entity.ID) return BadRequest(new { response = "ID mismatch" });

            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return Ok(new { response = "Success", data = entity });
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            LogContext($"Delete {typeof(T).Name}");
            var item = await _dbSet.FindAsync(id);
            if (item == null) return NotFound(new { response = "Not Found" });

            _dbSet.Remove(item);
            await context.SaveChangesAsync();

            return Ok(new { response = "Success" });
        }
    }
}