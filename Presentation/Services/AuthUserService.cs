using System.Security.Claims;

namespace Presentation.Services
{
    public class AuthUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public AuthUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Email => _accessor.HttpContext.User.Identity.Name;
        public string Name => GetClaimsIdentity().FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
        public string Id => GetClaimsIdentity().FirstOrDefault(a => a.Type == ClaimTypes.Sid)?.Value;

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
    }
}
