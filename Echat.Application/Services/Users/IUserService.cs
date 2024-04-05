using Echat.Application.ViewModels.Auth;
using Echat.Domain.Entities.Users;

namespace Echat.Application.Services.Users
{
    public interface IUserService
    {
        Task<bool> IsUserExist(string userName);
        Task<bool> IsUserExist(long userId);
        Task<bool> RegisterUser(RegisterViewModel registerModel);
        Task<User> LoginUser(LoginViewModel loginModel);
    }
}