using Presentation.Models;

namespace Presentation.Services.Interfaces
{
    public interface IUserService
    {
        bool IsLogged();
        Task<UserResponse> ApiLoginAsync(LoginModel model);
        Task<UserResponse> ApiRegisterAsync(RegisterModel model);
        Task FrontLoginAsync(HttpContext httpContext, UserResponse loginResponse);

    }
}
