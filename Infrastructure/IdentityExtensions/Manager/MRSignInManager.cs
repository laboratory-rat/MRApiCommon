using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRApiCommon.Infrastructure.IdentityExtensions.Components;

namespace MRMongoTools.Extensions.Identity.Manager
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class MRSignInManager<TUser> : SignInManager<TUser>
        where TUser : MRUser, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="claimsFactory"></param>
        /// <param name="optionsAccessor"></param>
        /// <param name="logger"></param>
        /// <param name="schemes"></param>
        public MRSignInManager(MRUserManager<TUser> userManager, IHttpContextAccessor contextAccessor,
                               IUserClaimsPrincipalFactory<MRUser> claimsFactory,
                               IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger,
                               IAuthenticationSchemeProvider schemes) : base(userManager, contextAccessor, null, optionsAccessor, logger, schemes)
        {
        }
    }
}
