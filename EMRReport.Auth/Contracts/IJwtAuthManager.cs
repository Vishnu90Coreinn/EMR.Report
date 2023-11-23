using EMRReport.Auth.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMRReport.Auth.Contracts
{
    public interface IJwtAuthManager
    {
        IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }

        JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now);

        JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now);

        void RemoveExpiredRefreshTokens(DateTime now);

        void RemoveRefreshTokenByUserName(string userName);

        (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);

    }
}
