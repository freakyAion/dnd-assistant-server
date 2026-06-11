using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dnd_assistant.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CampaignsController(MyDbContext context, ILogger<CampaignsController> logger)
        : TemplateController(context, logger)
    {
        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        [HttpGet("world/{worldId:guid}")]
        public async Task<IActionResult> GetWorldCampaigns(Guid worldId)
        {
            LogContext(nameof(GetWorldCampaigns));

            var campaigns = await _context.Campaigns
                .Where(c => c.WorldID == worldId)
                .Include(c => c.Characters)
                .Include(c => c.Sessions)
                .Select(c => new
                {
                    c.ID,
                    c.Name,
                    c.WorldID,
                    c.DungeonMasterID,
                    c.InviteCode,
                    c.Notes,
                    Characters = c.Characters.Select(ch => new { ch.ID, ch.Name }),
                    Sessions = c.Sessions.OrderBy(s => s.SessionNumber).Select(s => new
                    {
                        s.ID,
                        s.Name,
                        s.SessionNumber,
                        s.ScheduledAt,
                        s.Summary
                    })
                })
                .ToListAsync();

            return Ok(campaigns);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignDto dto)
        {
            LogContext(nameof(CreateCampaign));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var isWorldOwner = await _context.Worlds.AnyAsync(w => w.ID == dto.WorldId && w.OwnerID == userId.Value);
            if (!isWorldOwner) return Forbid();

            var campaign = new Campaign
            {
                Name = dto.Name,
                WorldID = dto.WorldId,
                DungeonMasterID = userId.Value,
                InviteCode = Guid.NewGuid().ToString()[..8].ToUpper()
            };

            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return Ok(campaign);
        }

        [AllowAnonymous]
        [HttpGet("invite/{code}")]
        public async Task<IActionResult> GetInviteDetails(string code)
        {
            LogContext(nameof(GetInviteDetails));

            var campaign = await _context.Campaigns
                .Include(c => c.World)
                .FirstOrDefaultAsync(c => c.InviteCode == code.ToUpper());

            if (campaign == null) return NotFound("Приглашение не найдено");

            return Ok(new
            {
                campaign.ID,
                campaign.Name,
                WorldName = campaign.World?.Name
            });
        }

        [HttpPost("invite/{code}/join")]
        public async Task<IActionResult> JoinCampaign(string code, [FromBody] JoinCampaignDto dto)
        {
            LogContext(nameof(JoinCampaign));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var campaign = await _context.Campaigns
                .Include(c => c.Characters)
                .FirstOrDefaultAsync(c => c.InviteCode == code.ToUpper());
            if (campaign == null) return NotFound("Кампания не найдена");

            var character = await _context.Characters
                .FirstOrDefaultAsync(ch => ch.ID == dto.CharacterId && ch.UserID == userId.Value);
            if (character == null) return BadRequest("Персонаж не найден");

            if (campaign.Characters.Any(ch => ch.ID == character.ID))
            {
                return BadRequest("Этот персонаж уже участвует в данной кампании");
            }

            character.CampaignID = campaign.ID;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Вы успешно присоединились к кампании!" });
        }

        [HttpPost("{campaignId:guid}/sessions")]
        public async Task<IActionResult> CreateSession(Guid campaignId, [FromBody] CreateSessionDto dto)
        {
            LogContext(nameof(CreateSession));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var campaign = await _context.Campaigns.FirstOrDefaultAsync(c => c.ID == campaignId);
            if (campaign == null) return NotFound("Кампания не найдена");
            if (campaign.DungeonMasterID != userId.Value) return Forbid();

            var lastSessionNumber = await _context.Sessions
                .Where(s => s.CampaignID == campaignId)
                .Select(s => (int?)s.SessionNumber)
                .MaxAsync() ?? 0;

            var session = new Session
            {
                CampaignID = campaignId,
                Name = dto.Name,
                SessionNumber = lastSessionNumber + 1,
                ScheduledAt = dto.ScheduledAt,
                Summary = string.Empty
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return Ok(session);
        }
    }

    public class CreateCampaignDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid WorldId { get; set; }
    }

    public class JoinCampaignDto
    {
        public Guid CharacterId { get; set; }
    }

    public class CreateSessionDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
    }
}