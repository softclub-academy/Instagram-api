using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.Dtos.LoginDto;
using Domain.Dtos.RegisterDto;
using Domain.Entities;
using Domain.Entities.User;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.AccountService;

public class AccountService : IAccountService
{
    private readonly IConfiguration _configuration;
    private readonly DataContext _context;

    public AccountService(IConfiguration configuration,
        DataContext context)
    {
        _configuration = configuration;
        _context = context;
    }
    
    public async Task<Response<string>> Register(RegisterDto model)
    {
        try
        {
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                UserType = model.UserType
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new Response<string>($"Done. Success! Your registered by id {user.UserId}");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> Login(LoginDto model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.UserName == model.UserName && u.Password == model.Password);
        if (user == null) return new Response<string>("Your username or password is incorrected!!!");
        return new Response<string>(await GenerateJwtToken(user));
    }
    
    private async Task<string> GenerateJwtToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };
        //add roles
        
        // var roles = await _userManager.GetRolesAsync(user);
        // claims.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role,role)));
        
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