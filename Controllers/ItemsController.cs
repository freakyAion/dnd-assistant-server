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
    public class ItemsController(MyDbContext context, ILogger<ItemsController> logger)
        : TemplateController(context, logger)
    {
        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            LogContext(nameof(GetById));

            var item = await _context.Items.FirstOrDefaultAsync(i => i.ID == id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] UpsertItemDto dto)
        {
            LogContext(nameof(Create));

            var jsonString = dto.Description?.ToString() ?? "{\"blocks\":[]}";
            using var doc = JsonDocument.Parse(jsonString);

            var item = new Item
            {
                Name = dto.Name,
                Type = dto.Type,
                Rarity = dto.Rarity,
                Description = JsonDocument.Parse(doc.RootElement.GetRawText()),
                Weight = dto.Weight,
                CostValue = dto.CostValue,
                CostCurrency = dto.CostCurrency,
                RequiresAttunement = dto.RequiresAttunement,
                AttunementPrerequisites = dto.AttunementPrerequisites,
                StrengthRequirement = dto.StrengthRequirement,
                StealthDisadvantage = dto.StealthDisadvantage,
                AcValue = dto.AcValue,
                AcDexBonusType = dto.AcDexBonusType,
                DamageDiceQuantity = dto.DamageDiceQuantity,
                DamageDiceSides = dto.DamageDiceSides,
                DamageType = dto.DamageType,
                Properties = dto.Properties,
                ContainerCapacityWeight = dto.ContainerCapacityWeight,
                IsConsumable = dto.IsConsumable,
                HasCharges = dto.HasCharges,
                MaxCharges = dto.MaxCharges,
                ChargeResetCondition = dto.ChargeResetCondition
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.ID }, item);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpsertItemDto dto)
        {
            LogContext(nameof(Update));

            var item = await _context.Items.FirstOrDefaultAsync(i => i.ID == id);
            if (item == null) return NotFound();

            var jsonString = dto.Description?.ToString() ?? "{\"blocks\":[]}";
            using var doc = JsonDocument.Parse(jsonString);

            item.Name = dto.Name;
            item.Type = dto.Type;
            item.Rarity = dto.Rarity;
            item.Description = JsonDocument.Parse(doc.RootElement.GetRawText());
            item.Weight = dto.Weight;
            item.CostValue = dto.CostValue;
            item.CostCurrency = dto.CostCurrency;
            item.RequiresAttunement = dto.RequiresAttunement;
            item.AttunementPrerequisites = dto.AttunementPrerequisites;
            item.StrengthRequirement = dto.StrengthRequirement;
            item.StealthDisadvantage = dto.StealthDisadvantage;
            item.AcValue = dto.AcValue;
            item.AcDexBonusType = dto.AcDexBonusType;
            item.DamageDiceQuantity = dto.DamageDiceQuantity;
            item.DamageDiceSides = dto.DamageDiceSides;
            item.DamageType = dto.DamageType;
            item.Properties = dto.Properties;
            item.ContainerCapacityWeight = dto.ContainerCapacityWeight;
            item.IsConsumable = dto.IsConsumable;
            item.HasCharges = dto.HasCharges;
            item.MaxCharges = dto.MaxCharges;
            item.ChargeResetCondition = dto.ChargeResetCondition;

            await _context.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            LogContext(nameof(Delete));

            var rowsAffected = await _context.Items.Where(i => i.ID == id).ExecuteDeleteAsync();
            if (rowsAffected == 0) return NotFound();

            return NoContent();
        }
    }

    public class UpsertItemDto
    {
        public string Name { get; set; } = string.Empty;
        public Item.ItemType Type { get; set; }
        public Item.ItemRarity Rarity { get; set; }
        public System.Text.Json.Nodes.JsonNode? Description { get; set; }
        public decimal Weight { get; set; }
        public int CostValue { get; set; }
        public Currency CostCurrency { get; set; }
        public bool RequiresAttunement { get; set; }
        public string? AttunementPrerequisites { get; set; }
        public int? StrengthRequirement { get; set; }
        public bool StealthDisadvantage { get; set; }
        public int? AcValue { get; set; }
        public Item.DexBonusType? AcDexBonusType { get; set; }
        public int? DamageDiceQuantity { get; set; }
        public int? DamageDiceSides { get; set; }
        public Item.WeaponDamageType? DamageType { get; set; }
        public Item.PropertyFlags Properties { get; set; }
        public decimal? ContainerCapacityWeight { get; set; }
        public bool IsConsumable { get; set; }
        public bool HasCharges { get; set; }
        public int? MaxCharges { get; set; }
        public string? ChargeResetCondition { get; set; }
    }
}