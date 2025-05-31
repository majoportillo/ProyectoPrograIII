using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SuperBodega.Admin.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly SuperBodegaDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SuperBodegaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserDto request)
        {
            if (await _context.Usuarios.AnyAsync(u => u.UsuarioNombre == request.UsuarioNombre))
                return BadRequest("El usuario ya existe");

            CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

            var usuario = new User
            {
                UsuarioNombre = request.UsuarioNombre,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuario registrado con éxito");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioNombre == request.UsuarioNombre);

            if (usuario == null)
                return BadRequest("Usuario no encontrado");

            if (!VerifyPasswordHash(request.Password, usuario.PasswordHash, usuario.PasswordSalt))
                return BadRequest("Contraseña incorrecta");

            var token = CreateToken(usuario);
            return Ok(new { token });
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }


        private String CreateToken(User user) { 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UsuarioNombre),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

    }
}
