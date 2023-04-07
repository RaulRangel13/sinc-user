using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Presentation.Models;
using Presentation.Services.Interfaces;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Presentation.Services
{
    public class UserService : IUserService
    {
        private readonly AuthUserService _user;

        public UserService(AuthUserService user)
        {
            _user = user;
        }

        public async Task<UserResponse> ApiLoginAsync(LoginModel model)
        {
            var url = "https://localhost:7086/api/Customer/SignIn";
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            return JsonConvert.DeserializeObject<UserResponse>(response.Content.ReadAsStringAsync().Result) ?? new UserResponse();
        }

        public async Task<UserResponse> ApiRegisterAsync(RegisterModel model)
        {
            var url = "https://localhost:7086/api/Customer/SigUp";
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            return JsonConvert.DeserializeObject<UserResponse>(response.Content.ReadAsStringAsync().Result) ?? new UserResponse();
        }

        public async Task FrontLoginAsync(HttpContext httpContext, UserResponse loginResponse)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, loginResponse.Name));
            claims.Add(new Claim(ClaimTypes.Sid, loginResponse.Id.ToString()));

            var userIdentity = new ClaimsIdentity(claims, "Acess");

            var principal = new ClaimsPrincipal(userIdentity);
            await httpContext.SignInAsync("CookieAuthentication", principal, new AuthenticationProperties());
        }

        public bool IsLogged()
        {
            if (_user.Id is null)
                return false;

            return true;
        }
    }
}
