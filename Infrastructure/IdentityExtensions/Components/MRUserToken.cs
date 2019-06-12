using System;

namespace MRApiCommon.Infrastructure.IdentityExtensions.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class MRUserToken
    {
        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ExpireTime { get; set; }
    }
}
