using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Presentation.Models;
using Presentation.Services.Interfaces;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Presentation.Services
{
    public class AuthenticationService : Presentation.Services.Interfaces.IAuthenticationService
    {
        private readonly AuthUserService _user;

        public AuthenticationService(AuthUserService user)
        {
            _user = user;
        }

        public AuthenticationService()
        {
        }

        public async Task<LoginResponse> ApiLoginAsync(LoginModel model)
        {
            var url = "https://localhost:7086/api/Customer/SignIn";
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            return JsonConvert.DeserializeObject<LoginResponse>(response.Content.ReadAsStringAsync().Result) ?? new LoginResponse();
        }

        public async Task FrontLoginAsync(HttpContext httpContext, LoginResponse loginResponse)
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
