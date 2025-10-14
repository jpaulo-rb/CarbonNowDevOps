using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarbonNowAPI.Model;
using Microsoft.IdentityModel.Tokens;

namespace CarbonNowAPI.Utils {
    public class TokenJWT {

        private readonly IConfiguration _config;

        public TokenJWT(IConfiguration config) {
            _config = config;
        }

        public string GerarToken(Usuario usuario) {

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Regra.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
