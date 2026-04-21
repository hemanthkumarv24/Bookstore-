using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bookstore_.Data;
using Bookstore_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bookstore_.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        if (InMemoryStore.Users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return BadRequest(new { message = "Email already exists." });
        }

        var role = request.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";
        var user = new User
        {
            Id = InMemoryStore.Users.Count == 0 ? 1 : InMemoryStore.Users.Max(u => u.Id) + 1,
            Name = request.Name,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = role
        };

        InMemoryStore.Users.Add(user);
        return Ok(new { user.Id, user.Name, user.Email, user.Role });
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var user = InMemoryStore.Users.FirstOrDefault(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        var token = CreateToken(user);
        if (!InMemoryStore.ActiveTokens.Contains(token))
        {
            InMemoryStore.ActiveTokens.Add(token);
        }

        return Ok(new { token, user = new { user.Id, user.Name, user.Email, user.Role } });
    }

    private string CreateToken(User user)
    {
        var jwt = configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiresInMinutes"]!)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public record RegisterRequest(string Name, string Email, string Password, string Role = "User");
    public record LoginRequest(string Email, string Password);
}
