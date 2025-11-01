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

        [HttpGet("{ID:guid}")]
        public async Task<IActionResult> GetByID([FromRoute] Guid ID)
        {
            LogContext(nameof(GetByID));

            if (ID == Guid.Empty) return BadRequest(new { response = "Empty ID" });

            User? user = await context.Users.FirstOrDefaultAsync(user => user.ID == ID);

            if (user == null) return NotFound(new
            {
                response = "User not found"
            });

            ReturnUserDTO returnedUser = new()
            {
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(new
            {
                response = "Success",
                user = returnedUser
            });
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] bool? all, [FromQuery] string? email, [FromQuery] string? name)
        {
            LogContext(nameof(Get));

            if (all == true)
            {
                // TODO?: Add a check for credentials

                List<User> users = await context.Users.ToListAsync();
                List<ReturnUserDTO> returnedUsers = [];
                foreach (var user in users)
                {
                    returnedUsers.Add(new ReturnUserDTO
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role
                    });
                }

                return Ok(new
                {
                    response = "Success",
                    count = returnedUsers.Count,
                    users = returnedUsers
                });
            }
            else if (!string.IsNullOrEmpty(email))
            {
                User? user = await context.Users.FirstOrDefaultAsync(user => user.Email == email);

                if (user == null) return NotFound(new
                {
                    response = "User not found"
                });

                ReturnUserDTO returnedUser = new()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                };

                return Ok(new
                {
                    response = "Success",
                    user = returnedUser
                });
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                List<User> users = await context.Users
                    .Where(user => user.Name == name)
                    .ToListAsync();
                List<ReturnUserDTO> returnedUsers = [];

                foreach (var user in users)
                {
                    returnedUsers.Add(new ReturnUserDTO
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role
                    });
                }

                return Ok(new
                {
                    response = "Success",
                    count = returnedUsers.Count,
                    users = returnedUsers
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = "All query parametres are empty"
                });
            }
        }
    }
}
