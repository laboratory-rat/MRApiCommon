using Microsoft.AspNetCore.Identity;
using MRApiCommon.Infrastructure.IdentityExtensions.Components;
using MRApiCommon.Infrastructure.Interface;

namespace MRApiCommon.Infrastructure.IdentityExtensions.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMRUserStore : IMRUserStore<MRUser> { }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="U"></typeparam>
    public interface IMRUserStore<U> :
        IMRRepository<U>,
        IUserStore<U>,
        IUserLoginStore<U>,
        IUserClaimStore<U>,
        IUserPasswordStore<U>,
        IUserEmailStore<U>,
        IUserTwoFactorStore<U>,
        IUserRoleStore<U>
        where U : MRUser, new()
    {
    }
}
