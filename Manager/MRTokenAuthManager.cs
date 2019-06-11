using Microsoft.AspNetCore.Http;
using MRApiCommon.Infrastructure.Common;
using System.Collections.Generic;
using System.Linq;

namespace MRApiCommon.Manager
{
    /// <summary>
    /// Base manager for MRToken auth
    /// </summary>
    public abstract class MRTokenAuthManager
    {
        /// <summary>
        /// Http context accessor
        /// </summary>
        protected IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Is user authorized
        /// </summary>
        protected bool _isAuthorized => _httpContextAccessor.HttpContext?.User != null;

        /// <summary>
        /// Get current user id
        /// </summary>
        protected string _userId => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == MRTokenDefaults.CLAIM_USER_ID).Value;

        /// <summary>
        /// Get current user email
        /// </summary>
        protected string _userEmail => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == MRTokenDefaults.CLAIM_USER_EMAIL).Value;

        /// <summary>
        /// Get current user roles
        /// </summary>
        protected IEnumerable<string> _userRoles => _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == MRTokenDefaults.CLAIM_USER_ROLE).Select(x => x.Value);

        /// <summary>
        /// Is current user in role
        /// </summary>
        /// <param name="role">Target role</param>
        /// <returns></returns>
        protected bool IsInRole(string role) => _userRoles.Contains(role);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public MRTokenAuthManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
