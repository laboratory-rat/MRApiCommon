using System.Collections.Generic;

namespace MRApiCommon.Infrastructure.Model.Token
{
    /// <summary>
    /// Default respondent of MR token validation
    /// </summary>
    public class MRCorrectTokenData
    {
        /// <summary>
        /// Decrypted user id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Decrypted user email
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Decrypted user roles
        /// </summary>
        public IEnumerable<string> UserRoles { get; set; }
    }
}
