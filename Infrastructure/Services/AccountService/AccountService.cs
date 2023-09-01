using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.Dtos.LoginDto;
using Domain.Dtos.RegisterDto;
using Domain.Entities.User;
using Domain.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.AccountService;

public class AccountService : IAccountService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountService(IConfiguration configuration,
        UserManager<IdentityUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<Response<string>> Register(RegisterDto model)
    {
        try
        {
            var result = await _userManager.FindByNameAsync(model.UserName);
            if (result != null) return new Response<string>(HttpStatusCode.BadRequest, "Such a user already exists!");
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                UserType = model.UserType,
                DateRegistred = DateTime.UtcNow
            };
            await _userManager.CreateAsync(user, model.Password);
            return new Response<string>($"Done.  Your registered by id {user.Id}");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> Login(LoginDto model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, model.Password);
                if (result)
                    return new Response<string>(await GenerateJwtToken(user));
            }
            return new Response<string>("Your username or password is incorrect!!!");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sid, user.Id),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            
        };
        //add roles
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        var securityTokenHandler = new JwtSecurityTokenHandler();
        var tokenString = securityTokenHandler.WriteToken(token);
        return tokenString;
    }
}