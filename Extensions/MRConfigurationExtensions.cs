using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MRApiCommon.Infrastructure.Common;
using MRApiCommon.Middleware;
using MRApiCommon.Options;

namespace MRApiCommon.Extensions
{
    /// <summary>
    /// Configuration extensions
    /// </summary>
    public static class MRConfigurationExtensions
    {
        /// <summary>
        /// Add MRExceptionMiddleware for app
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureMRExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<MRExceptionMiddleware>();
        }

        /// <summary>
        /// Config JWT Token with options and DI Options[MRTokenOptions]
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        public static void ConfigureMRToken(this IServiceCollection services, MRTokenOptions options)
        {
            services.AddOptions<MRTokenOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = options.RequireHttps;
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = options.ValidateIssuer,
                        ValidIssuer = options.Issuer,
                        ValidateAudience = options.ValidateAudience,
                        ValidAudience = options.Audience,
                        ValidateLifetime = options.ValidateLifetime,
                        IssuerSigningKey = options.GetSecurityKey,
                        ValidateIssuerSigningKey = options.ValidateKey,
                        NameClaimType = MRTokenDefaults.CLAIM_USER_ID,
                        RoleClaimType = MRTokenDefaults.CLAIM_USER_ROLE
                    };
                });
        }
    }
}
