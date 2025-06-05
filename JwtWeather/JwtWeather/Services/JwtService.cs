using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;


using JwtWeather.Services;

public class JwtService
{
   private readonly string _SECRETKEY;
   private readonly IConfiguration _configuration;
   

   public JwtService(IConfiguration configuration)
   {    
        _configuration = configuration;
        _SECRETKEY = _configuration["Jwt:SecretKey"] ?? 
         throw new ArgumentNullException("JWT secret key is missing in appsettings.json");
   }

   public string GenerateToken(string username)
   {
        var claim = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_SECRETKEY));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
               issuer: _configuration["Jwt:Issuer"],
               audience: _configuration["Jwt:Audience"],
               claims: claim,
               expires: DateTime.Now.AddHours(1),
               signingCredentials: cred);

        return new JwtSecurityTokenHandler().WriteToken(token);
   }
}