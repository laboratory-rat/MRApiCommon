using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRApiCommon.Infrastructure.Common;
using MRApiCommon.Infrastructure.IdentityExtensions.Components;
using MRApiCommon.Infrastructure.IdentityExtensions.Interface;
using MRApiCommon.Infrastructure.IdentityExtensions.Store;
using MRApiCommon.Middleware;
using MRApiCommon.Options;
using MRApiCommon.Service;
using MRMongoTools.Extensions.Identity.Manager;
using System;

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
        /// <param name="configuration"></param>
        /// <param name="tokenOptionsKey"></param>
        public static void ConfigureMRToken(this IServiceCollection services, IConfiguration configuration, string tokenOptionsKey)
        {
            var options = new MRTokenOptions();
            configuration.Bind(tokenOptionsKey, options);
            services.Configure<MRTokenOptions>(configuration.GetSection(tokenOptionsKey));

            services.AddTransient<MRTokenService>();

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

        /// <summary>
        /// Configurate MR Identity with MongoDB in system
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="dbOptionsKey"></param>
        /// <param name="userSignupActions"></param>
        public static void ConfigurateMRIdentity<TUser, TUserStore, TUserManager>(this IServiceCollection services, IConfiguration configuration, string dbOptionsKey, Action<IdentityOptions> userSignupActions = null)
            where TUser : MRUser, new()
            where TUserStore : MRUserStore<TUser>
            where TUserManager : MRUserManager<TUser>
        {
            var dbOptions = new MRDbOptions();
            configuration.Bind(dbOptionsKey, dbOptions);
            services.Configure<MRDbOptions>(configuration.GetSection(dbOptionsKey));

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IMRUserStore<TUser>, TUserStore>();
            services.AddTransient<IMRRoleStore, MRRoleStore>();
            services.AddTransient<IUserValidator<TUser>, MRUserValidator<TUser>>();

            services.AddTransient<MRRoleManager>();
            services.AddTransient<TUserManager>();
            services.AddTransient<MRSignInManager<TUser>>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            userSignupActions = userSignupActions ?? new Action<IdentityOptions>((a) => {
                a.User.RequireUniqueEmail = true;
            });

            services.AddIdentityCore<TUser>(userSignupActions)
                .AddDefaultTokenProviders();
        }

        /// <summary>
        /// Default interpretation
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="dbOptionsKey"></param>
        /// <param name="userSignupActions"></param>
        public static void ConfigurateMRIdentity(this IServiceCollection services, IConfiguration configuration, string dbOptionsKey, Action<IdentityOptions> userSignupActions = null)
         => ConfigurateMRIdentity<MRUser, MRUserStore, MRUserManager<MRUser>>(services, configuration, dbOptionsKey, userSignupActions);
    }

}
