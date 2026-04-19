using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthorizationAPI.Extenshions
{
    public static class JwtServiceExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            return AddJwtAuthentication(services, configuration, "JwtSettings");
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration, string sectionName)
        {
            var jwtSettings = configuration.GetSection(sectionName);
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException($"JWT SecretKey not configured in section '{sectionName}'");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = false,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["Es-cookies"];
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();

            return services;
        }
    }
}
