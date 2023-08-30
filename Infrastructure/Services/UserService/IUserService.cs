using System.Security.Cryptography;
using Domain.Dtos.UserDto;
using Domain.Dtos.UserLogDto;
using Domain.Filters;
using Domain.Filters.UserFilter;
using Domain.Responses;

namespace Infrastructure.Services.UserService;

public interface IUserService
{
    Task<PagedResponse<List<GetUserDto>>> GetUsers(UserFilter filter);
    Task<Response<GetUserDto>> GetUserById(int id);
    Task<Response<string>> UserRegister(AddUserDto addUser);
    Task<Response<string>> UserLogin(UserLoginDto addUser);
    Task<Response<string>> UserLogout(int id);
    Task<Response<GetUserDto>> UpdateUser(AddUserDto addUser);
    Task<Response<bool>> DeleteUser(int id);
    Task<PagedResponse<List<GetUserLogDto>>> GetUserLogsByUserId(UserLogFilter filter);
    Task<Response<GetUserLogDto>> GetUserLogById(int id);
}