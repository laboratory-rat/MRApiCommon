using System;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MRApiCommon.Infrastructure.Attr;
using MRApiCommon.Infrastructure.Enum;
using MRApiCommon.Infrastructure.Interface;

namespace MRApiCommon.Infrastructure.IdentityExtensions.Components
{
    /// <summary>
    /// 
    /// </summary>
    [CollectionAttr("Role")]
    public class MRRole : IdentityRole, IMREntity
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MREntityState State { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public void GenerateKey()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
