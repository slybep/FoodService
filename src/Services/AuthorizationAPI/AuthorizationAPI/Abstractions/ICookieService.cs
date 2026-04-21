namespace AuthorizationAPI.Abstractions
{
    public interface ICookieService
    {
        void SetAuthCookie(string token);
    }
}