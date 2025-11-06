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
    public class UserController(MyDbContext context, IHttpContextAccessor contextAccessor, ILogger<TemplateController> logger, IPasswordHasher<User> passwordHasher, JWTService jwtService) : TemplateController(context, contextAccessor, logger)
    {
        private readonly IPasswordHasher<User> passwordHasher = passwordHasher;
        private readonly JWTService jwtService = jwtService;

        // TODO (Security):
        // 1. Enforce HTTPS across the entire API.
        // 2. Ensure no logs capture raw passwords (request bodies, exceptions, middleware).
        // 3. Verify that only hashed passwords (PBKDF2 via IPasswordHasher) are stored.
        // 4. Confirm that no password values are ever returned in responses.
        // 5. Add password validation rules and rate limiting to reduce automated abuse.

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
                nameof(Get),
                new { id = newUser.ID },
                new
                {
                    response = "Success",
                    user = returnedUser
                }
            );

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] bool? all, [FromQuery] Guid ID, [FromQuery] string? email, [FromQuery] string? name)
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
            else if (ID != Guid.Empty)
            {
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

        // TODO: Add Update PUT

        // TODO: Add Delete

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            LogContext(nameof(Login));

            /*if (!ModelState.IsValid)
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
            }*/

            User? user = await context.Users.FirstOrDefaultAsync(findUser => findUser.Email == userDTO.Email);

            if (user == null) return Unauthorized(new
            {
                response = "Wrong email or password"
            });

            PasswordVerificationResult verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userDTO.Password);
            if (verificationResult == PasswordVerificationResult.Failed) return Unauthorized(new
            {
                response = "Wrong email or password"
            });

            if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, userDTO.Password);
                user.Touch();
                context.Users.Update(user);
            }

            string accessToken = jwtService.GenerateAccessToken(user, length: 15);
            var (rawRefreshToken, hashedRefreshToken) = jwtService.CreateRefreshToken();

            string? ip = contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var refreshToken = new RefreshToken
            {
                UserID = user.ID,
                TokenHash = hashedRefreshToken,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(14),
                CreatedByIp = ip
            };

            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.Expires,
            };

            Response.Cookies.Append("refreshToken", rawRefreshToken, cookieOptions);

            var returnedUser = new ReturnUserDTO
            {
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(new
            {
                response = "Success",
                accessToken,
                user = returnedUser
            });

        }
    }
}
