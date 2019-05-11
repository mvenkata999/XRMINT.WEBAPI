using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using CarXRMWebAPI.Models;
using CarXRMWebAPI.Options;
using CarXRMWebAPI.Repository;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;

namespace CarXRMWebAPI.Factory
{
    public class JwtFactory : IJwtFactory
    {
        private ILoginRepository _loginRepository = null;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtFactory(IOptions<JwtIssuerOptions>jwtOptions, ILoginRepository loginRepository, IHttpContextAccessor httpContextAccessor)
        {
            _jwtOptions = jwtOptions.Value;
            _loginRepository = loginRepository;
            ThrowIfInvalidOptions(_jwtOptions);

            _httpContextAccessor = httpContextAccessor;
        }

        public Task<ClaimsIdentity> GetClaimsIdentity(ILoginUser loginUser)
        { 
            try
            {
                IApiUser apiUser = _loginRepository.CheckUser(loginUser.UserName, loginUser.Password);
                if (apiUser == null)
                {
                    return Task.FromResult<ClaimsIdentity>(null);
                }

                var userClaims = new List<Claim>();             
                userClaims.Add(new Claim("UserId", apiUser.UserId.ToString()));
                userClaims.Add(new Claim(ClaimTypes.Name, apiUser.UserName));

                string dealerIds = string.Empty;
                var dealerList = apiUser.DealerList.ToList();
                foreach (Dealer dealer in dealerList)
                {
                    dealerIds += dealer.DealerId + ",";
                }

                if (dealerIds.LastIndexOf(',') == dealerIds.Length-1)
                    dealerIds = dealerIds.Remove(dealerIds.LastIndexOf(','));

                if (dealerIds.Length > 0)
                {
                    userClaims.Add(new Claim("DealerIds", dealerIds));
                }

                string roleIds = string.Empty;
                var roleList = apiUser.RoleList.ToList();
                foreach (Role role in roleList)
                {
                    roleIds += role.RoleId + ",";
                }

                if (roleIds.LastIndexOf(',') == roleIds.Length-1)
                    roleIds = roleIds.Remove(roleIds.LastIndexOf(','));

                if (roleIds.Length > 0)
                {
                    userClaims.Add(new Claim("RoleIds", roleIds));
                }

                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(loginUser.UserName, "Token"), userClaims));

            }
            catch(Exception ex)
            {
                //error handler
            }
            
            return Task.FromResult<ClaimsIdentity>(null);            
        }

        private void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new List<Claim>(identity.Claims);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, identity.Name));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64));            
            
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
    }
}
