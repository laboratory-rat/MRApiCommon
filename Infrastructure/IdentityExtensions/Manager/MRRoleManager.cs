using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MRApiCommon.Infrastructure.IdentityExtensions.Components;
using MRApiCommon.Infrastructure.IdentityExtensions.Interface;

namespace MRMongoTools.Extensions.Identity.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public class MRRoleManager : RoleManager<MRRole>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="store"></param>
        /// <param name="roleValidators"></param>
        /// <param name="keyNormalizer"></param>
        /// <param name="errors"></param>
        /// <param name="logger"></param>
        public MRRoleManager(IMRRoleStore store, IEnumerable<IRoleValidator<MRRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<MRRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
