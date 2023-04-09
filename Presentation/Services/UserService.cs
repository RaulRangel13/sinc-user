using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Presentation.Models;
using Presentation.Services.Interfaces;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Presentation.Services
{
    public class UserService : IUserService
    {
        private readonly AuthUserService _user;
        private readonly IConfiguration _configuration;

        public UserService(AuthUserService user,
            IConfiguration configuration)
        {
            _user = user;
            _configuration = configuration;
        }

        public async Task<UserResponse> ApiChangePasswordAsync(RecoverModel model)
        {
            //var url = "https://localhost:7086/api/Customer/ChangePassword";
            var url = _configuration.GetValue<string>("UserApp:Host") + "Customer/ChangePassword";
            var json = $"{{\"newPassword\":\"{model.Password}\"}}";
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Token);
            var response = await client.PostAsync(url, data);
            return JsonConvert.DeserializeObject<UserResponse>(response.Content.ReadAsStringAsync().Result) ?? new UserResponse();
        }

        public async Task<UserResponse> AutorizationToken(string token)
        {
            //var url = "https://localhost:7086/api/Token/AutorizationToken";
            var url = _configuration.GetValue<string>("UserApp:Host") + "Token/AutorizationToken";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(url);
            return JsonConvert.DeserializeObject<UserResponse>(response.Content.ReadAsStringAsync().Result) ?? new UserResponse();
        }

        public async Task<UserResponse> ApiRecoverEmailPassAsync(RecoverPasswordModel model)
        {
            //var url = $"https://localhost:7086/api/Customer/RecoverPassword/";
            var url = _configuration.GetValue<string>("UserApp:Host") + "Customer/RecoverPassword/";
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            return JsonConvert.DeserializeObject<UserResponse>(response.Content.ReadAsStringAsync().Result) ?? new UserResponse();
        }

        public async Task<UserResponse> ApiTokenLoginAsync(RecoverModel model)
        {
            //var url = "https://localhost:7086/api/Customer/SignIn";
            var url = _configuration.GetValue<string>("UserApp:Host") + "Customer/SignIn";
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            return JsonConvert.DeserializeObject<UserResponse>(response.Content.ReadAsStringAsync().Result) ?? new UserResponse();
        }

        public async Task<UserResponse> ApiLoginAsync(LoginModel model)
        {
            //var url = "https://localhost:7086/api/Customer/SigIn";
            var url = _configuration.GetValue<string>("UserApp:Host") + "Customer/SigIn";
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            return JsonConvert.DeserializeObject<UserResponse>(response.Content.ReadAsStringAsync().Result) ?? new UserResponse();
        }

        public async Task<UserResponse> ApiRegisterAsync(RegisterModel model)
        {
            //var url = "https://localhost:7086/api/Customer/SigUp";
            var url = _configuration.GetValue<string>("UserApp:Host") + "Customer/SigUp";
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            return JsonConvert.DeserializeObject<UserResponse>(response.Content.ReadAsStringAsync().Result) ?? new UserResponse();
        }

        public async Task FrontLoginAsync(HttpContext httpContext, UserResponse loginResponse)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, loginResponse.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, loginResponse.Name));
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

        public async Task<bool> ApiGenerateKeyAsync(int id)
        {
            //var url = $"https://localhost:7086/api/TwoFa/GenerateKey/{id}";
            var url = _configuration.GetValue<string>("UserApp:Host") + $"TwoFa/GenerateKey/{id}";
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

        public async Task<bool> ApiValidateKeyAsync(TwoFaValidateModel validateModel)
        {
            //var url = "https://localhost:7086/api/TwoFa/ValidateKey";
            var url = _configuration.GetValue<string>("UserApp:Host") + "TwoFa/ValidateKey";
            var json = JsonConvert.SerializeObject(validateModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }
    }
}
