using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.ServiceContracts;

namespace Relaxinema.Core.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration config, UserManager<User> userManager)
    {
        _configuration = config;
        _userManager = userManager;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
    }

    public async Task<string> CreateTokenAsync(User user)
    {
        var roles = string.Join(",", await _userManager.GetRolesAsync(user));
        
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roles),
            };
        
        // claims.AddRange((await _userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };
        
        JwtSecurityToken tokenGenerator = new(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var tokenHandler = new JwtSecurityTokenHandler();

        //var token = tokenHandler.WriteToken(tokenGenerator);

        return tokenHandler.WriteToken(tokenGenerator);
    }
}
