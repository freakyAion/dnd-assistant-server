using dnd_assistant.DB;
using dnd_assistant.DTOs;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dnd_assistant.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger, IPasswordHasher<User> passwordHasher) : TemplateController(context, contextAccessor, logger)
    {
        private readonly IPasswordHasher<User> passwordHasher = passwordHasher;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO userDTO)
        {
            LogContext(nameof(Create));

            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState
                    .Where(fieldState => fieldState.Value!.Errors.Count > 0)
                    .ToDictionary(
                        fieldState => fieldState.Key,
                        fieldState => fieldState.Value!.Errors
                            .Select(error => error.ErrorMessage)
                            .ToList()
                    );

                return BadRequest(new
                {
                    response = "Validation Error",
                    errors = validationErrors
                });
            }

            User? existingUser = await context.Users.FirstOrDefaultAsync(findUser => findUser.Email == userDTO.Email);

            if (existingUser != null) return Conflict(new
            {
                response = "Duplicate Entries"
            });

            User newUser = new()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                PasswordHash = "",
                Role = userDTO.Role
            };

            newUser.PasswordHash = passwordHasher.HashPassword(newUser, userDTO.Password);

            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            var returnedUser = new ReturnUserDTO
            {
                Name = newUser.Name,
                Email = newUser.Email,
                Role = newUser.Role
            };

            return CreatedAtAction(
                nameof(GetByID),
                new { id = newUser.ID },
                new
                {
                    response = "Success",
                    user = returnedUser
                }
            );

        }

        [HttpGet("{id}")]
        //public async Task<IActionResult> GetByID(Guid ID)
        public void GetByID()
        {

        }
    }
}
