using dnd_assistant.DB;
using dnd_assistant.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace dnd_assistant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger) : TemplateController(context, contextAccessor, logger)
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnClassDTO>>> GetClasses()
        {
            var emptyJson = JsonDocument.Parse("{}").RootElement;

            var classDtos = await context.Classes.Select(c => new ReturnClassDTO
            {
                Name = c.Name,
                Description = c.Description ?? emptyJson,
                HitDie = c.HitDie,
                SkillProficienciesAmount = c.SkillProficienciesAmount,
                SubClassUnlockingLevel = c.SubClassUnlockingLevel,
                ArmourProficiencies = c.ArmourProficiencies,
                WeaponProficiencies = c.WeaponProficiencies,
                SavingThrowProficiencies = c.SavingThrowProficiencies,
                SpellcastingAbility = c.SpellcastingAbility,

                Features = c.Features.Select(f => new FeatureDTO
                {
                    Name = f.Name,
                    Description = f.Description ?? emptyJson,
                    UnlockingLevel = f.UnlockingLevel
                }).ToList(),

                SpecificWeaponProficiencies = c.SpecificWeaponProficiencies.Select(wp => new WeaponProficiencyDTO
                {
                    WeaponID = wp.WeaponID,
                    Weapon = wp.Weapon
                }).ToList(),

                SpellSlotProgression = c.SpellSlotProgression.Select(ssp => new SpellSlotProgressionDTO
                {
                    ClassLevel = ssp.ClassLevel,
                    Slot1 = ssp.Slot1,
                    Slot2 = ssp.Slot2,
                    Slot3 = ssp.Slot3,
                    Slot4 = ssp.Slot4,
                    Slot5 = ssp.Slot5,
                    Slot6 = ssp.Slot6,
                    Slot7 = ssp.Slot7,
                    Slot8 = ssp.Slot8,
                    Slot9 = ssp.Slot9,
                    CantripsKnown = ssp.CantripsKnown,
                    SpellsKnown = ssp.SpellsKnown,
                    PactSlotLevel = ssp.PactSlotLevel,
                    PactSlotCount = ssp.PactSlotCount
                }).ToList(),

                SkillPool = c.SkillPool.Select(sp => sp.Skill).ToList()
            }).ToListAsync();

            return Ok(classDtos);
        }

    }
}