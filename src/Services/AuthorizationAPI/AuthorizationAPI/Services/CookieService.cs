using AuthorizationAPI.Abstractions;

namespace AuthorizationAPI.Services
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CookieService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public void SetAuthCookie(string token)
        {
            var context = _httpContextAccessor.HttpContext;
            context?.Response.Cookies.Append("Es-cookies", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(10),
                Path = "/"                     
            });
        }
    }
}
