using Microsoft.AspNet.Identity;
using MRApiCommon.Infrastructure.Attr;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Interface;

namespace MRApiCommon.Infrastructure.IdentityExtensions.Components
{
    /// <summary>
    /// 
    /// </summary>
    [CollectionAttr("Role")]
    public class MRRole : MREntity, IMREntity, IRole
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NormalizedName { get; set; }
    }
}
