using dnd_assistant.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace dnd_assistant
{
    public class JWTService(string key, string issuer, string audience)
    {
        public string GenerateAccessToken(User user, int length = 15)
        {
            string roleString = user.Role.ToString();

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, roleString),
                new Claim("name", user.Name)
            ];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(length),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string rawToken, string hashedToken) CreateRefreshToken(int sizeBytes = 64)
        {
            byte[] randomBytes = new byte[sizeBytes];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            string rawToken = Convert.ToBase64String(randomBytes);

            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
            string hashedToken = Convert.ToBase64String(hashBytes);

            return (rawToken, hashedToken);
        }
    }
}
