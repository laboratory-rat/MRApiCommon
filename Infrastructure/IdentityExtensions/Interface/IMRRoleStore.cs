using Microsoft.AspNetCore.Identity;
using MRApiCommon.Infrastructure.IdentityExtensions.Components;
using MRApiCommon.Infrastructure.Interface;

namespace MRApiCommon.Infrastructure.IdentityExtensions.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMRRoleStore : IMRRepository<MRRole>, IRoleStore<MRRole> { }
}
