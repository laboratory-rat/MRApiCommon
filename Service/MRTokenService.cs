using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MRApiCommon.Exception;
using MRApiCommon.Infrastructure.Common;
using MRApiCommon.Infrastructure.Model.Token;
using MRApiCommon.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace MRApiCommon.Service
{
    /// <summary>
    /// Service for manipulations with toknes
    /// </summary>
    public class MRTokenService
    {
        protected readonly MRTokenOptions _options;
        protected ILogger _logger;

        public MRTokenService(IOptions<MRTokenOptions> options, ILoggerFactory loggerFactory)
        {
            _options = options.Value;
            _logger = loggerFactory?.CreateLogger(GetType());
        }

        /// <summary>
        /// Generate token based on MRTokenOptions data
        /// </summary>
        /// <param name="userId">Target user id</param>
        /// <param name="userEmail">Target user email</param>
        /// <param name="userRoles">Target user role list</param>
        /// <returns>JWT token</returns>
        public string GenerateToken(string userId, string userEmail, IEnumerable<string> userRoles)
        {
            var claimData = new List<Tuple<string, string>>
            {
                new Tuple<string, string>(MRTokenDefaults.CLAIM_USER_ID, userId),
                new Tuple<string, string>(MRTokenDefaults.CLAIM_USER_EMAIL, userEmail),
            };

            userRoles?.ToList().ForEach(role => claimData.Add(new Tuple<string, string>(MRTokenDefaults.CLAIM_USER_ROLE, role)));

            return GenerateToken(_options.Key, _options.Issuer, _options.Audience, _options.Lifetime, claimData, MRTokenDefaults.CLAIM_USER_ID, MRTokenDefaults.CLAIM_USER_ROLE);
        }

        /// <summary>
        /// Generate custom token
        /// </summary>
        /// <param name="key">Token key</param>
        /// <param name="issuer">Token issuer</param>
        /// <param name="audience">Token audience</param>
        /// <param name="expireMinutes">Token expire time in minutes</param>
        /// <param name="claimData">Token claim data</param>
        /// <param name="nameClaim">Token name claim type</param>
        /// <param name="roleClaim">Token role claim type</param>
        /// <returns>JWT token</returns>
        public string GenerateToken(string key, string issuer, string audience, int expireMinutes, List<Tuple<string, string>> claimData, string nameClaim, string roleClaim)
        {
            // Generate claims identity
            var claims = new List<Claim>();
            foreach (var tuple in claimData)
            {
                claims.Add(new Claim(tuple.Item1, tuple.Item2));
            }

            var identity = new ClaimsIdentity(claims, "Token", nameClaim, roleClaim);

            // generate token
            var now = DateTime.UtcNow;
            var expires = expireMinutes > 0 ? now.Add(TimeSpan.FromMinutes(expireMinutes)) : now.Add(TimeSpan.FromMinutes(60));

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                notBefore: now,
                expires: expires,
                claims: identity.Claims,
                signingCredentials: new SigningCredentials(_options.GenerateSecurityKey(key), SecurityAlgorithms.HmacSha256));
            var encoded = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encoded;
        }

        /// <summary>
        /// Validate MRToken with MRTokenOptions data
        /// </summary>
        /// <param name="token">Target token</param>
        /// <returns><see cref="MRCorrectTokenData"/></returns>
        public MRCorrectTokenData ValidateToken(string token)
        {
            var parsed = ValidateToken(
                token,
                _options.Key,
                _options.Audience,
                _options.Issuer,
                5,
                _options.ValidateAudience,
                _options.ValidateIssuer,
                _options.ValidateLifetime);

            if (parsed == null)
                return null;

            return new MRCorrectTokenData
            {
                UserId = parsed.Claims.FirstOrDefault(x => x.Type == MRTokenDefaults.CLAIM_USER_ID)?.Value,
                UserEmail = parsed.Claims.FirstOrDefault(x => x.Type == MRTokenDefaults.CLAIM_USER_EMAIL)?.Value,
                UserRoles = parsed.Claims.Where(x => x.Type == MRTokenDefaults.CLAIM_USER_ROLE).Select(x => x.Value)
            };
        }

        /// <summary>
        /// Validate custom token
        /// </summary>
        /// <param name="token">Target token</param>
        /// <param name="key">Token key</param>
        /// <param name="audience">Correct audience</param>
        /// <param name="issuer">Correct issuer</param>
        /// <param name="clockSkewMinutes">Clock skew</param>
        /// <param name="validateAudience">Provide audience validation</param>
        /// <param name="validateIssuer">Provide issuer validation</param>
        /// <param name="validateLifetime">Provide lifetime validation</param>
        /// <returns></returns>
        public JwtSecurityToken ValidateToken(string token, string key, string audience, string issuer, int clockSkewMinutes = 5, bool validateAudience = true, bool validateIssuer = true, bool validateLifetime = true)
        {
            clockSkewMinutes = Math.Max(clockSkewMinutes, 1);

            var validator = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.FromMinutes(clockSkewMinutes),
                IssuerSigningKeys = new List<SecurityKey> { _options.GenerateSecurityKey(key) },
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateLifetime = validateLifetime,
                ValidateAudience = validateAudience,
                ValidateIssuer = validateIssuer,
                ValidAudience = audience,
                ValidIssuer = issuer,
            };

            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler()
                    .ValidateToken(token, validator, out var rawValidatedToken);

                var securityToken = (JwtSecurityToken)rawValidatedToken;
                return securityToken;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Generate Identity based on MRTokenOptions file
        /// </summary>
        /// <param name="userId">Target user id</param>
        /// <param name="userEmail">Target user email</param>
        /// <param name="userRoles">Target user role list</param>
        /// <returns><see cref="ClaimsIdentity"/></returns>
        public ClaimsIdentity GenerateClaimsIdentity(string userId, string userEmail, IEnumerable<string> userRoles)
        {
            var claims = new List<Claim>
            {
                new Claim(MRTokenDefaults.CLAIM_USER_ID, userId),
                new Claim(MRTokenDefaults.CLAIM_USER_EMAIL, userEmail),
            };

            if (userRoles != null && userRoles.Any())
            {
                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(MRTokenDefaults.CLAIM_USER_ROLE, role));
                }
            }

            return new ClaimsIdentity(claims, "Token", MRTokenDefaults.CLAIM_USER_ID, MRTokenDefaults.CLAIM_USER_ROLE);
        }
    }
}
