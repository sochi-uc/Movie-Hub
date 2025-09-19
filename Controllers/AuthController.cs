using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Movie_Hub.Entities;
using Movie_Hub.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movie_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        public static User user = new();

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            var hasehedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.Username  = request.Username;
            user.PasswordHash = hasehedPassword;

            return Ok(user);
        }
        [HttpPost("login")]
        public ActionResult<string> Login(UserDto request)
        {
            if (user.Username != request.Username)
            {
                return BadRequest("User not found");
            }
            var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Wrong password");
            }
            string token = CreateToken(user);
            return Ok(token);
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username)
    };

            // get secret key from configuration
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            // generate token string
            var tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenDescriptor);

            return token;
        }

    }
}
