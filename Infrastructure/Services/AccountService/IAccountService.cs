using Domain.Dtos.LoginDto;
using Domain.Dtos.RegisterDto;
using Domain.Responses;

namespace Infrastructure.Services.AccountService;

public interface IAccountService
{
    Task<Response<string>> Register(RegisterDto model);
    Task<Response<string>> Login(LoginDto model);
}