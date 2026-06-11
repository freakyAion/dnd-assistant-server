using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace dnd_assistant.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class WorldsController(MyDbContext context, ILogger<WorldsController> logger)
        : TemplateController(context, logger)
    {
        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (Guid.TryParse(userIdClaim, out var userId)) return userId;
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyWorlds()
        {
            LogContext(nameof(GetMyWorlds));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var worlds = await _context.Worlds
                .Where(w => w.OwnerID == userId.Value)
                .Select(w => new
                {
                    w.ID,
                    w.Name,
                    w.IsPublic,
                    CampaignCount = w.Campaigns.Count
                })
                .OrderBy(w => w.Name)
                .ToListAsync();

            return Ok(worlds);
        }

        [HttpGet("joined")]
        public async Task<IActionResult> GetJoinedWorlds()
        {
            LogContext(nameof(GetJoinedWorlds));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var joinedWorlds = await _context.Worlds
                .Where(w => w.Campaigns.Any(camp => camp.Characters.Any(ch => ch.UserID == userId.Value)))
                .Select(w => new
                {
                    w.ID,
                    w.Name,
                    w.IsPublic,
                    CampaignCount = w.Campaigns.Count
                })
                .OrderBy(w => w.Name)
                .ToListAsync();

            return Ok(joinedWorlds);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetWorldDetails(Guid id)
        {
            LogContext(nameof(GetWorldDetails));

            var world = await _context.Worlds
                .AsNoTracking()
                .Include(w => w.Npcs)
                .Include(w => w.Locations)
                .Include(w => w.HistoricalEvents)
                .AsSplitQuery()
                .FirstOrDefaultAsync(w => w.ID == id);

            if (world == null) return NotFound();
            var userId = GetCurrentUserId();

            if (!world.IsPublic)
            {

                if (userId == null) return Forbid();

                if (world.OwnerID != userId.Value && !User.IsInRole("Admin"))
                {
                    var isPlayer = await _context.Campaigns
                        .Where(c => c.WorldID == id)
                        .AnyAsync(c => c.Characters.Any(ch => ch.UserID == userId.Value));

                    if (!isPlayer) return Forbid();
                }
            }

            return Ok(new
            {
                world.ID,
                world.Name,
                world.OwnerID,
                world.Description,
                world.IsPublic,
                world.MapImageUrl,
                Locations = world.Locations.Select(l => new
                {
                    l.ID,
                    l.Name,
                    l.Type,
                    l.Description,
                    l.X,
                    l.Y,
                    l.ParentLocationID
                }),
                Npcs = world.Npcs.Select(n => new
                {
                    n.ID,
                    n.Name,
                    n.Race,
                    n.Occupation,
                    n.Alignment,
                    n.Description,
                    n.IsSecret
                }),
                HistoricalEvents = world.HistoricalEvents.Select(h => new
                {
                    h.ID,
                    h.Name,
                    h.DateOrEra,
                    h.Description
                })
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorld([FromBody] CreateWorldDto dto)
        {
            LogContext(nameof(CreateWorld));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var world = new World
            {
                OwnerID = userId.Value,
                Name = dto.Name,
                Description = dto.Description ?? JsonNode.Parse("{\"blocks\":[]}"),
                IsPublic = dto.IsPublic
            };

            _context.Worlds.Add(world);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWorldDetails), new { id = world.ID }, new
            {
                world.ID,
                world.Name,
                world.OwnerID,
                world.Description,
                world.IsPublic
            });
        }

        [HttpPost("{worldId:guid}/locations")]
        public async Task<IActionResult> AddLocation(Guid worldId, [FromBody] CreateLocationDto dto)
        {
            LogContext(nameof(AddLocation));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var world = await _context.Worlds.AnyAsync(w => w.ID == worldId && w.OwnerID == userId.Value);
            if (!world) return Forbid();

            var location = new Location
            {
                WorldID = worldId,
                Name = dto.Name,
                Type = dto.Type,
                Description = dto.Description ?? JsonNode.Parse("{\"blocks\":[]}"),
                ParentLocationID = dto.ParentLocationId,
                X = dto.X,
                Y = dto.Y
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                location.ID,
                location.Name,
                location.Type,
                location.Description,
                location.ParentLocationID,
                location.X,
                location.Y
            });
        }

        [HttpPut("{id:guid}/map")]
        public async Task<IActionResult> UpdateWorldMap(Guid id, [FromBody] UpdateWorldMapDto dto)
        {
            LogContext(nameof(UpdateWorldMap));
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var rowsAffected = await _context.Worlds
                .Where(w => w.ID == id && w.OwnerID == userId.Value)
                .ExecuteUpdateAsync(setters => setters.SetProperty(w => w.MapImageUrl, dto.MapImageUrl));

            if (rowsAffected == 0) return Forbid();

            return Ok();
        }

        [HttpPost("{worldId:guid}/npcs")]
        public async Task<IActionResult> AddNpc(Guid worldId, [FromBody] CreateNpcDto dto)
        {
            LogContext(nameof(AddNpc));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            // Verify that the world exists and is actually owned by the current user
            var worldExists = await _context.Worlds.AnyAsync(w => w.ID == worldId && w.OwnerID == userId.Value);
            if (!worldExists) return Forbid();

            var npc = new Npc
            {
                WorldID = worldId,
                Name = dto.Name,
                Race = dto.Race ?? string.Empty,
                Occupation = dto.Occupation ?? string.Empty,
                Alignment = dto.Alignment ?? "Neutral",
                Description = dto.Description ?? JsonNode.Parse("{\"blocks\":[]}"),
                IsSecret = dto.IsSecret
            };

            _context.Npcs.Add(npc);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                npc.ID,
                npc.WorldID,
                npc.Name,
                npc.Race,
                npc.Occupation,
                npc.Alignment,
                npc.Description,
                npc.IsSecret
            });
        }

        [HttpPut("{worldId:guid}/npcs/{npcId:guid}")]
        public async Task<IActionResult> UpdateNpc(Guid worldId, Guid npcId, [FromBody] UpdateNpcDto dto)
        {
            LogContext(nameof(UpdateNpc));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var worldExists = await _context.Worlds.AnyAsync(w => w.ID == worldId && w.OwnerID == userId.Value);
            if (!worldExists) return Forbid();

            var npc = await _context.Npcs.FirstOrDefaultAsync(n => n.ID == npcId && n.WorldID == worldId);
            if (npc == null) return NotFound();

            npc.Name = dto.Name;
            npc.Race = dto.Race ?? string.Empty;
            npc.Occupation = dto.Occupation ?? string.Empty;
            npc.Alignment = dto.Alignment ?? "Neutral";
            npc.Description = dto.Description ?? npc.Description;
            npc.IsSecret = dto.IsSecret;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                npc.ID,
                npc.WorldID,
                npc.Name,
                npc.Race,
                npc.Occupation,
                npc.Alignment,
                npc.Description,
                npc.IsSecret
            });
        }

        [HttpPut("{id:guid}/description")]
        public async Task<IActionResult> UpdateWorldDescription(Guid id, [FromBody] UpdateWorldDescriptionDto dto)
        {
            LogContext(nameof(UpdateWorldDescription));
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var fallbackDescription = JsonNode.Parse("{\"blocks\":[]}", null, default);

            var rowsAffected = await _context.Worlds
                .Where(w => w.ID == id && w.OwnerID == userId.Value)
                .ExecuteUpdateAsync(setters => setters.SetProperty(
                    w => w.Description,
                    dto.Description ?? fallbackDescription));

            if (rowsAffected == 0) return Forbid();

            return Ok();
        }

        [HttpPost("{worldId:guid}/history")]
        public async Task<IActionResult> AddHistoryEvent(Guid worldId, [FromBody] CreateHistoryEventDto dto)
        {
            LogContext(nameof(AddHistoryEvent));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var isOwner = await _context.Worlds.AnyAsync(w => w.ID == worldId && w.OwnerID == userId.Value);
            if (!isOwner) return Forbid();

            var historyEvent = new WorldEvent
            {
                WorldID = worldId,
                Name = dto.Name,
                DateOrEra = dto.DateOrEra,
                Description = dto.Description ?? JsonNode.Parse("{\"blocks\":[]}")
            };

            _context.WorldEvents.Add(historyEvent);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                historyEvent.ID,
                historyEvent.Name,
                historyEvent.DateOrEra,
                historyEvent.Description
            });
        }

        [HttpPut("history/{id:guid}")]
        public async Task<IActionResult> UpdateHistoryEvent(Guid id, [FromBody] UpdateHistoryEventDto dto)
        {
            LogContext(nameof(UpdateHistoryEvent));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var historyEvent = await _context.WorldEvents
                .Include(h => h.World)
                .FirstOrDefaultAsync(h => h.ID == id);

            if (historyEvent == null) return NotFound();
            if (historyEvent.World?.OwnerID != userId.Value) return Forbid();

            historyEvent.Name = dto.Name;
            historyEvent.DateOrEra = dto.DateOrEra;
            historyEvent.Description = dto.Description ?? JsonNode.Parse("{\"blocks\":[]}");

            await _context.SaveChangesAsync();

            return Ok(new
            {
                historyEvent.ID,
                historyEvent.Name,
                historyEvent.DateOrEra,
                historyEvent.Description
            });
        }

        [HttpDelete("history/{id:guid}")]
        public async Task<IActionResult> DeleteHistoryEvent(Guid id)
        {
            LogContext(nameof(DeleteHistoryEvent));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var historyEvent = await _context.WorldEvents
                .Include(h => h.World)
                .FirstOrDefaultAsync(h => h.ID == id);

            if (historyEvent == null) return NotFound();
            if (historyEvent.World?.OwnerID != userId.Value) return Forbid();

            _context.WorldEvents.Remove(historyEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

    #region Data Transfer Objects (DTOs)

    public class CreateWorldDto
    {
        public string Name { get; set; } = string.Empty;
        public JsonNode? Description { get; set; }
        public bool IsPublic { get; set; }
    }

    public class CreateLocationDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public JsonNode? Description { get; set; }
        public Guid? ParentLocationId { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
    }

    public class UpdateWorldMapDto
    {
        public string MapImageUrl { get; set; } = string.Empty;
    }

    public class CreateNpcDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Race { get; set; }
        public string? Occupation { get; set; }
        public string? Alignment { get; set; }
        public JsonNode? Description { get; set; }
        public bool IsSecret { get; set; }
    }

    public class UpdateNpcDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Race { get; set; }
        public string? Occupation { get; set; }
        public string? Alignment { get; set; }
        public JsonNode? Description { get; set; }
        public bool IsSecret { get; set; }
    }

    public class UpdateWorldDescriptionDto
    {
        public JsonNode? Description { get; set; }
    }

    public class CreateHistoryEventDto
    {
        public string Name { get; set; } = string.Empty;
        public string DateOrEra { get; set; } = string.Empty;
        public JsonNode? Description { get; set; }
    }

    public class UpdateHistoryEventDto
    {
        public string Name { get; set; } = string.Empty;
        public string DateOrEra { get; set; } = string.Empty;
        public JsonNode? Description { get; set; }
    }

    #endregion
}