using Domain.Dtos.UserDto;
using Domain.Filters.UserFilter;
using Domain.Responses;

namespace Infrastructure.Services.UserService;

public interface IUserService
{
    Task<PagedResponse<List<GetUserDto>>> GetUsers(UserFilter filter);
    Task<Response<GetUserDto>> GetUserById(string id);
    Task<Response<GetUserDto>> UpdateUser(AddUserDto addUser);
    Task<Response<bool>> DeleteUser(string id);
}