using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MRApiCommon.Infrastructure.Attr;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Enum;
using MRApiCommon.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace MRApiCommon.Infrastructure.IdentityExtensions.Components
{
    /// <summary>
    /// 
    /// </summary>
    [CollectionAttr("User")]
    public class MRUser : MREntity, IMREntity, IUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NormalizedUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public MRUserSex Sex { get; set; } = MRUserSex.UNDEFINED;

        /// <summary>
        /// 
        /// </summary>
        public string NormalizedEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Isblocked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BlockReason { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int FailedLoginCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MRUserImage Image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<MRUserTel> Tels { get; set; } = new List<MRUserTel>();

        /// <summary>
        /// 
        /// </summary>
        public List<Claim> Claims { get; set; } = new List<Claim>();

        /// <summary>
        /// 
        /// </summary>
        public List<MRUserToken> Tokens { get; set; } = new List<MRUserToken>();

        /// <summary>
        /// 
        /// </summary>
        public List<Microsoft.AspNetCore.Identity.UserLoginInfo> Logins { get; set; } = new List<Microsoft.AspNetCore.Identity.UserLoginInfo>();

        /// <summary>
        /// 
        /// </summary>
        public List<MRUserRole> Roles { get; set; } = new List<MRUserRole>();

        /// <summary>
        /// 
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 
        /// </summary>
        public bool TwoFactorEnabled { get; set; }
    }
}
