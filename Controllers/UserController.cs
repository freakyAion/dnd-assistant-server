using dnd_assistant.DB;
using dnd_assistant.DTOs;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace dnd_assistant.Controllers
{
    [Route("api/[controller]")]
    public class UsersController(MyDbContext context, JWTService jwtService, ILogger<UsersController> logger)
        : TemplateController(context, logger)
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            LogContext(nameof(Register));

            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == request.Email.ToLower()))
            {
                return BadRequest(new { message = "Email address is already registered." });
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = UserRole.Player
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            LogContext(nameof(Login));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var accessToken = jwtService.GenerateAccessToken(user);
            var (rawRefresh, hashedRefresh) = jwtService.CreateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                TokenHash = hashedRefresh,
                UserID = user.ID,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            SetRefreshTokenCookie(rawRefresh);

            return Ok(new AuthResponse(
                AccessToken: accessToken,
                RefreshToken: rawRefresh,
                Name: user.Name,
                Email: user.Email,
                Role: user.Role.ToString()
            ));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            LogContext(nameof(Refresh));

            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshToken));
            string incomingHash = Convert.ToBase64String(hashBytes);

            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.TokenHash == incomingHash);

            if (storedToken == null || storedToken.Expires < DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token." });
            }

            var user = storedToken.User;
            var newAccessToken = jwtService.GenerateAccessToken(user);
            var (newRawRefresh, newHashedRefresh) = jwtService.CreateRefreshToken();

            storedToken.TokenHash = newHashedRefresh;
            storedToken.Expires = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
            SetRefreshTokenCookie(newRawRefresh);

            return Ok(new AuthResponse(
                AccessToken: newAccessToken,
                RefreshToken: newRawRefresh,
                Name: user.Name,
                Email: user.Email,
                Role: user.Role.ToString()
            ));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            LogContext(nameof(Logout));

            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshToken));
            string incomingHash = Convert.ToBase64String(hashBytes);

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == incomingHash);
            if (storedToken != null)
            {
                _context.RefreshTokens.Remove(storedToken);
                await _context.SaveChangesAsync();
            }

            Response.Cookies.Delete("refreshToken");
            return Ok(new { message = "Logged out successfully." });
        }

        private void SetRefreshTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}