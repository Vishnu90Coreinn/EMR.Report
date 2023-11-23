using EMRReport.Auth.Contracts;
using EMRReport.Auth.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EMRReport.Auth
{
    public sealed class JwtAuthManager : IJwtAuthManager
    {
        private readonly JwtTokenConfig _jwtTokenConfig;
        private readonly byte[] _secret;
        public IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }

        private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;

        #region Constructors & Destructor

        public JwtAuthManager(JwtTokenConfig jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig;
            _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
            _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
        }

        #endregion

        public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new SecurityTokenException("Auth_Invalid_Token_Error");
            }


            var tokenParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtTokenConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_secret),
                ValidAudience = _jwtTokenConfig.Audience,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenParameters, out var validatedToken);
            return (principal, validatedToken as JwtSecurityToken);
        }

        public JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now)
        {
            var shouldAddAudienceClaim =
                string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
            var jwtToken = new JwtSecurityToken(_jwtTokenConfig.Issuer,
                shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty, claims,
                expires: now.AddHours(_jwtTokenConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret),
                    SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var accessexpireDate = now.AddHours(_jwtTokenConfig.AccessTokenExpiration);
            var refreshToken = new RefreshToken
            {
                UserName = username,
                TokenString = GenerateRefreshTokenString(),
                ExpireAt = now.AddHours(_jwtTokenConfig.RefreshTokenExpiration)
            };

            _usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);
            return new JwtAuthResult { AccessToken = accessToken, RefreshToken = refreshToken, AccessTokenExpiry = accessexpireDate };
        }

        public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Auth_Invalid_Token_Error");
            }

            var userName = principal.Identity.Name;
            if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
            {
                throw new SecurityTokenException("Auth_Invalid_Token_Error");
            }

            if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpireAt < now)
            {
                throw new SecurityTokenException("Auth_Invalid_Token_Error");
            }

            return GenerateTokens(userName, principal.Claims.ToArray(), now);
        }

        public void RemoveExpiredRefreshTokens(DateTime now)
        {
            var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpireAt < now).ToList();
            foreach (var expiredToken in expiredTokens)
            {
                _usersRefreshTokens.Remove(expiredToken.Key, out _);
            }
        }

        public void RemoveRefreshTokenByUserName(string userName)
        {
            var refreshTokens = _usersRefreshTokens.Where(x => x.Value.UserName == userName)
                .ToList();
            foreach (var refreshToken in refreshTokens)
            {
                _usersRefreshTokens.Remove(refreshToken.Key, out _);
            }
        }
        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
