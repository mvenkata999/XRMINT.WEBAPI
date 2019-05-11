using CarXRMWebAPI.Factory;
using CarXRMWebAPI.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models.Security
{
    public interface IJwtToken
    {
        Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings);
    }
}
