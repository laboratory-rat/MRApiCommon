using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRApiCommon.Infrastructure.IdentityExtensions.Components;
using MRApiCommon.Infrastructure.IdentityExtensions.Interface;

namespace MRMongoTools.Extensions.Identity.Manager
{
    public class MRUserManager<TUser> : UserManager<TUser>
        where TUser : MRUser, new()
    {
        public MRUserManager(
            IMRUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, 
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger) 
                : base(store, null, passwordHasher, null, null, keyNormalizer, null, services, logger)
        {
        }
    }
}
