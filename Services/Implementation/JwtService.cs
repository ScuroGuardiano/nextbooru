using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace UltraHornyBoard.Services.Implementation;

public class JwtService : IJwtService
{

    private AppSettings.JwtSettings configuration;

    public JwtService(IOptions<AppSettings> configuration)
    {
        this.configuration = configuration.Value.Jwt;
    }

    public string SignToken(string subject)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var claims = new List<Claim>() {
            new("sub", subject)
        };

        var token = new JwtSecurityToken(
            configuration.Issuer,
            configuration.Audience,
            claims,
            DateTime.Now,
            DateTime.Now.Add(configuration.JwtExpirationTS),
            creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}