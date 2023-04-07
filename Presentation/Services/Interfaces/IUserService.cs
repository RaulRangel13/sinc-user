using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Services.Interfaces
{
    public interface IUserService
    {
        bool IsLogged();
        Task<UserResponse> ApiChangePasswordAsync(RecoverModel model);
        Task<UserResponse> ApiLoginAsync(LoginModel model);
        Task<UserResponse> ApiTokenLoginAsync(RecoverModel model);
        Task<UserResponse> AutorizationToken(string token);
        Task<UserResponse> ApiRecoverEmailPassAsync(RecoverPasswordModel model);
        Task<UserResponse> ApiRegisterAsync(RegisterModel model);
        Task FrontLoginAsync(HttpContext httpContext, UserResponse loginResponse);

    }
}
