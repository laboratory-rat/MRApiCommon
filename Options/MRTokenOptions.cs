using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace MRApiCommon.Options
{
    /// <summary>
    /// Basic token options
    /// [Provide it in AppSettings.json]
    /// </summary>
    public class MRTokenOptions
    {
        /// <summary>
        /// Token issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Provide issuer validation
        /// </summary>
        public bool ValidateIssuer { get; set; }

        /// <summary>
        /// Token audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Provide audience validation
        /// </summary>
        public bool ValidateAudience { get; set; }

        /// <summary>
        /// Secret key for token
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Provide key validation
        /// </summary>
        public bool ValidateKey { get; set; }

        /// <summary>
        /// Tokne lifetime in minutes
        /// </summary>
        public int Lifetime { get; set; }

        /// <summary>
        /// Provide token lifetime validation
        /// </summary>
        public bool ValidateLifetime { get; set; }

        /// <summary>
        /// Require HTTPS connection for token transfer
        /// </summary>
        public bool RequireHttps { get; set; }

        /// <summary>
        /// Get Symmetric key from options key
        /// </summary>
        [JsonIgnore]
        public SymmetricSecurityKey GetSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));

        /// <summary>
        /// Generate custom not settings provided security key
        /// </summary>
        /// <param name="key">Security key</param>
        /// <returns>Symmetric key</returns>
        public SymmetricSecurityKey GenerateSecurityKey(string key) => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
    }
}
