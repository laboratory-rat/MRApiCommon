using Microsoft.AspNetCore.Identity;
using MRApiCommon.Infrastructure.IdentityExtensions.Components;
using System.Threading.Tasks;

namespace MRMongoTools.Extensions.Identity.Manager
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    class MRUserValidator<TUser> : UserValidator<TUser>, IUserValidator<TUser>
        where TUser : MRUser, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errors"></param>
        public MRUserValidator(IdentityErrorDescriber errors = null) : base(errors) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
            => Task.FromResult(IdentityResult.Success);
    }
}
