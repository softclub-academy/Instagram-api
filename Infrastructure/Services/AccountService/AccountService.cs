using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.Dtos;
using Domain.Dtos.LoginDto;
using Domain.Dtos.MessageDto;
using Domain.Dtos.MessagesDto;
using Domain.Dtos.RegisterDto;
using Domain.Entities.User;
using Domain.Enums;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Text;

namespace Infrastructure.Services.AccountService;

public class AccountService : IAccountService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly  DataContext _dbContext;

    private readonly IEmailService _emailService;


    public AccountService(IConfiguration configuration,
        UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, DataContext dbContext, IEmailService emailService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
        _emailService = emailService;
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
            var profile = new UserProfile()
            {
                UserId = user.Id,
                FirstName = string.Empty,
                LastName = string.Empty,
                Occupation = string.Empty,
                DateUpdated = DateTime.UtcNow,
                LocationId = 1,
                DOB = DateTime.UtcNow,
                Image = string.Empty,
                About = string.Empty,
                Gender = Gender.Female,
            };

            await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user,Roles.User);
            await _dbContext.UserProfiles.AddAsync(profile);
            await _dbContext.SaveChangesAsync();
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
            return new Response<string>(HttpStatusCode.BadRequest, "Your username or password is incorrect!!!");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var userProfile = await _dbContext.UserProfiles.FindAsync(user.Id);
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sid, user.Id),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            //new Claim(JwtRegisteredClaimNames.Sub, userProfile.Image),
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
    
    
    
    
     public async Task<Response<string>> ChangePassword(ChangePasswordDto passwordDto, string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);

            var checkPassword = await _userManager.CheckPasswordAsync(user!, passwordDto.OldPassword);
            if (checkPassword == false)
            {
                return new Response<string>(HttpStatusCode.BadRequest, "password is incorrect");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user!);
            var result = await _userManager.ResetPasswordAsync(user!, token, passwordDto.Password);
            if (result.Succeeded == true)
                return new Response<string>(HttpStatusCode.OK, "success");
            else return new Response<string>(HttpStatusCode.BadRequest, "could not reset your password");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> ForgotPasswordTokenGenerator(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var existing = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (existing == null) return new Response<string>(HttpStatusCode.BadRequest, "email  not found");
            var token = await _userManager.GeneratePasswordResetTokenAsync(existing);
            var url = $"http://localhost:5271/account/resetpassword?token={token}&email={forgotPasswordDto.Email}";
            _emailService.SendEmail(
                new MessagesDto(new[] { forgotPasswordDto.Email }, "reset password",
                    $"<h1><a href=\"{url}\">reset password</a></h1>"), TextFormat.Html);

            return new Response<string>(HttpStatusCode.OK, "reset password has been sent");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<string>> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return new Response<string>(HttpStatusCode.BadRequest, "user not found");

            var resetPassResult =
                await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (resetPassResult.Succeeded)
                return new Response<string>(HttpStatusCode.OK, "success");

            return new Response<string>(HttpStatusCode.BadRequest, "please try again");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.BadRequest, e.Message);
        }

    }

}