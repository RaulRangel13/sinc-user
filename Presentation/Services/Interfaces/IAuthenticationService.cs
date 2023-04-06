using Presentation.Models;

namespace Presentation.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> ApiLoginAsync(LoginModel model);
        Task FrontLoginAsync(HttpContext httpContext, LoginResponse loginResponse);
        bool IsLogged(); 
    }
}
