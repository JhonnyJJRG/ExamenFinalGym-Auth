using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ExamenFinalGym.Data;
using ExamenFinalGym.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore; // Necesario para usar .Include()

namespace ExamenFinalGym.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Usuario login)
        {
            
            var user = _context.Usuarios
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.UserName == login.UserName && u.PasswordHash == login.PasswordHash);

            if (user == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas" });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), 
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var userRole in user.UserRoles)
            {
                if (userRole.Role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}