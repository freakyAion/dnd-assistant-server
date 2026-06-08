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
    public class CharactersController(MyDbContext context, ILogger<CharactersController> logger)
        : TemplateController(context, logger)
    {
        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCharacters()
        {
            LogContext(nameof(GetMyCharacters));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var characters = await _context.Characters
                .Where(c => c.UserID == userId.Value)
                .Select(c => new
                {
                    c.ID,
                    c.Name,
                    ClassName = c.Class.Name,
                    SpeciesName = c.Species.Name,
                    c.Level,
                    c.Alignment
                })
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Ok(characters);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCharacterSheet(Guid id)
        {
            LogContext(nameof(GetCharacterSheet));

            var character = await _context.Characters
                .Include(c => c.Species).ThenInclude(s => s.Traits)
                .Include(c => c.Background).ThenInclude(b => b.Features)
                .Include(c => c.Class).ThenInclude(cl => cl.Progressions)
                .Include(c => c.Inventory).ThenInclude(ci => ci.Item)
                .Include(c => c.Spells).ThenInclude(cs => cs.Spell)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (character == null) return NotFound();

            if (character.IsPublic)
            {
                return Ok(character);
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(userIdClaim) || (character.UserID.ToString() != userIdClaim && !User.IsInRole("Admin")))
            {
                return Forbid();
            }

            return Ok(character);
        }

        [HttpPut("{id:guid}/visibility")]
        public async Task<IActionResult> UpdateVisibility(Guid id, [FromBody] UpdateVisibilityDto dto)
        {
            LogContext(nameof(UpdateVisibility));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var character = await _context.Characters.FirstOrDefaultAsync(c => c.ID == id);
            if (character == null) return NotFound();
            if (character.UserID != userId.Value) return Forbid();

            character.IsPublic = dto.IsPublic;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacter([FromBody] CreateCharacterDto dto)
        {
            LogContext(nameof(CreateCharacter));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var speciesExists = await _context.Species.AnyAsync(s => s.ID == dto.SpeciesID);
            var backgroundExists = await _context.Backgrounds.AnyAsync(b => b.ID == dto.BackgroundID);
            var classExists = await _context.Classes.AnyAsync(c => c.ID == dto.ClassID);

            if (!speciesExists || !backgroundExists || !classExists)
            {
                return BadRequest("Invalid Species, Background, or Class reference selection.");
            }

            var character = new Character
            {
                UserID = userId.Value,
                Name = dto.Name,
                SpeciesID = dto.SpeciesID,
                BackgroundID = dto.BackgroundID,
                ClassID = dto.ClassID,
                Level = 1,
                ExperiencePoints = 0,
                Alignment = dto.Alignment,
                Biography = dto.Biography,
                Age = dto.Age,
                Height = dto.Height,
                Weight = dto.Weight,
                PhysicalAppearance = dto.PhysicalAppearance,
                Strength = dto.Strength,
                Dexterity = dto.Dexterity,
                Constitution = dto.Constitution,
                Intelligence = dto.Intelligence,
                Wisdom = dto.Wisdom,
                Charisma = dto.Charisma,
                MaxHitPoints = dto.MaxHitPoints,
                CurrentHitPoints = dto.MaxHitPoints,
                TemporaryHitPoints = 0,
                ExpendedSpellSlots = new int[9],
                ActiveConditions = System.Text.Json.JsonDocument.Parse("[]")
            };

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCharacterSheet), new { id = character.ID }, character);
        }

        [AllowAnonymous]
        [HttpGet("classes")]
        public async Task<IActionResult> GetWizardClasses()
        {
            LogContext(nameof(GetWizardClasses));
            var classes = await _context.Classes
                .Select(c => new { c.ID, c.Name })
                .OrderBy(c => c.Name)
                .ToListAsync();
            return Ok(classes);
        }

        [AllowAnonymous]
        [HttpGet("species")]
        public async Task<IActionResult> GetWizardSpecies()
        {
            LogContext(nameof(GetWizardSpecies));
            var species = await _context.Species
                .Select(s => new { s.ID, s.Name })
                .OrderBy(s => s.Name)
                .ToListAsync();
            return Ok(species);
        }

        [AllowAnonymous]
        [HttpGet("backgrounds")]
        public async Task<IActionResult> GetWizardBackgrounds()
        {
            LogContext(nameof(GetWizardBackgrounds));
            var backgrounds = await _context.Backgrounds
                .Select(b => new { b.ID, b.Name })
                .OrderBy(b => b.Name)
                .ToListAsync();
            return Ok(backgrounds);
        }

        [HttpPut("{id:guid}/biography")]
        public async Task<IActionResult> UpdateCharacterBio(Guid id, [FromBody] UpdateBioDto dto)
        {
            LogContext(nameof(UpdateCharacterBio));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var character = await _context.Characters.FirstOrDefaultAsync(c => c.ID == id);
            if (character == null) return NotFound();
            if (character.UserID != userId.Value) return Forbid();

            character.Alignment = dto.Alignment;
            character.Biography = dto.Biography;
            character.Age = dto.Age;
            character.Height = dto.Height;
            character.Weight = dto.Weight;
            character.PhysicalAppearance = dto.PhysicalAppearance;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:guid}/vitals")]
        public async Task<IActionResult> UpdateVitals(Guid id, [FromBody] UpdateVitalsDto dto)
        {
            LogContext(nameof(UpdateVitals));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var character = await _context.Characters.FirstOrDefaultAsync(c => c.ID == id);
            if (character == null) return NotFound();
            if (character.UserID != userId.Value) return Forbid();

            character.CurrentHitPoints = dto.CurrentHitPoints;
            character.MaxHitPoints = dto.MaxHitPoints;
            character.TemporaryHitPoints = dto.TemporaryHitPoints;
            character.DeathSaveSuccesses = dto.DeathSaveSuccesses;
            character.DeathSaveFailures = dto.DeathSaveFailures;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:guid}/inventory")]
        public async Task<IActionResult> UpdateInventoryItem(Guid id, [FromBody] UpdateInventoryDto dto)
        {
            LogContext(nameof(UpdateInventoryItem));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var character = await _context.Characters.FirstOrDefaultAsync(c => c.ID == id);
            if (character == null) return NotFound();
            if (character.UserID != userId.Value) return Forbid();

            var characterItem = await _context.CharacterItems
                .FirstOrDefaultAsync(ci => ci.CharacterID == id && ci.ItemID == dto.ItemID);

            if (characterItem == null)
            {
                if (dto.Quantity <= 0) return BadRequest("Cannot add item with zero or negative quantity.");

                characterItem = new CharacterItem
                {
                    CharacterID = id,
                    ItemID = dto.ItemID,
                    Quantity = dto.Quantity,
                    IsEquipped = dto.IsEquipped,
                    IsAttuned = dto.IsAttuned
                };
                _context.CharacterItems.Add(characterItem);
            }
            else
            {
                if (dto.Quantity <= 0)
                {
                    _context.CharacterItems.Remove(characterItem);
                }
                else
                {
                    characterItem.Quantity = dto.Quantity;
                    characterItem.IsEquipped = dto.IsEquipped;
                    characterItem.IsAttuned = dto.IsAttuned;
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:guid}/spell-slots")]
        public async Task<IActionResult> UpdateSpellSlots(Guid id, [FromBody] UpdateSpellSlotsDto dto)
        {
            LogContext(nameof(UpdateSpellSlots));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var character = await _context.Characters.FirstOrDefaultAsync(c => c.ID == id);
            if (character == null) return NotFound();
            if (character.UserID != userId.Value) return Forbid();

            if (dto.ExpendedSpellSlots.Length != 9) return BadRequest("Spell slots tracking sequence array size must be exactly 9 items.");

            character.ExpendedSpellSlots = dto.ExpendedSpellSlots;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:guid}/spells/preparation")]
        public async Task<IActionResult> ToggleSpellPreparation(Guid id, [FromBody] ToggleSpellDto dto)
        {
            LogContext(nameof(ToggleSpellPreparation));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var character = await _context.Characters.FirstOrDefaultAsync(c => c.ID == id);
            if (character == null) return NotFound();
            if (character.UserID != userId.Value) return Forbid();

            var characterSpell = await _context.CharacterSpells
                .FirstOrDefaultAsync(cs => cs.CharacterID == id && cs.SpellID == dto.SpellID);

            if (characterSpell == null)
            {
                characterSpell = new CharacterSpell
                {
                    CharacterID = id,
                    SpellID = dto.SpellID,
                    IsPrepared = dto.IsPrepared,
                    IsAlwaysPrepared = false
                };
                _context.CharacterSpells.Add(characterSpell);
            }
            else
            {
                characterSpell.IsPrepared = dto.IsPrepared;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCharacter(Guid id)
        {
            LogContext(nameof(DeleteCharacter));

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var character = await _context.Characters.FirstOrDefaultAsync(c => c.ID == id);
            if (character == null) return NotFound();
            if (character.UserID != userId.Value && !User.IsInRole("Admin")) return Forbid();

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    #region Data Transfer Objects (DTOs)

    public class CreateCharacterDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid SpeciesID { get; set; }
        public Guid BackgroundID { get; set; }
        public Guid ClassID { get; set; }
        public string Alignment { get; set; } = "Neutral";
        public string Biography { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string PhysicalAppearance { get; set; } = string.Empty;
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Constitution { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int Charisma { get; set; } = 10;
        public int MaxHitPoints { get; set; } = 10;
    }

    public class UpdateBioDto
    {
        public string Alignment { get; set; } = "Neutral";
        public string Biography { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string PhysicalAppearance { get; set; } = string.Empty;
    }

    public class UpdateVitalsDto
    {
        public int CurrentHitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int TemporaryHitPoints { get; set; }
        public int DeathSaveSuccesses { get; set; }
        public int DeathSaveFailures { get; set; }
    }

    public class UpdateInventoryDto
    {
        public Guid ItemID { get; set; }
        public int Quantity { get; set; }
        public bool IsEquipped { get; set; }
        public bool IsAttuned { get; set; }
    }

    public class UpdateSpellSlotsDto
    {
        public int[] ExpendedSpellSlots { get; set; } = new int[9];
    }

    public class ToggleSpellDto
    {
        public Guid SpellID { get; set; }
        public bool IsPrepared { get; set; }
    }

    public class UpdateVisibilityDto
    {
        public bool IsPublic { get; set; }
    }

    #endregion
}