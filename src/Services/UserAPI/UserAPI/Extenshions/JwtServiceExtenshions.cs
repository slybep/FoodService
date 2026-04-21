using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtServiceExtenshions
{
    private readonly RequestDelegate _next;
    private readonly string _secretKey;

    public JwtServiceExtenshions(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _secretKey = config["JwtSettings:SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey not configured");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine("[JWT Middleware] Request path: " + context.Request.Path);
        var token = context.Request.Cookies["Es-cookies"] ??
                    context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

        Console.WriteLine($"[JWT Middleware] Token from cookie/header: {token?.Substring(0, Math.Min(20, token?.Length ?? 0))}...");
        Console.WriteLine($"[JWT Middleware] Token length: {token?.Length}");

        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("[JWT Middleware] No token found, continuing without authentication.");
            await _next(context);
            return;
        }
        if (!token.Contains('.'))
        {
            Console.WriteLine("[JWT Middleware] Token does not contain dots, aborting.");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid token format (no dots).");
            return;
        }

        try
        {

            Console.WriteLine("[JWT Middleware] Creating TokenValidationParameters...");
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5),
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.NameIdentifier
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var claims = principal.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "CustomJwt");
            context.User = new ClaimsPrincipal(identity);
            Console.WriteLine($"[JWT Middleware] User ID: {principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value}");

            context.User = principal;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[JWT Middleware] Validation failed: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"[JWT Middleware] Inner exception: {ex.InnerException.Message}");

            context.Response.StatusCode = 401;
            await context.Response.WriteAsync($"Invalid token: {ex.Message}");
            return;
        }

        await _next(context);
    }
}