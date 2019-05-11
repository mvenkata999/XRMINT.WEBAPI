using CarXRMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Factory
{
    public interface IJwtFactory
    {
        Task<ClaimsIdentity> GetClaimsIdentity(ILoginUser apiUser);
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
    }
}
