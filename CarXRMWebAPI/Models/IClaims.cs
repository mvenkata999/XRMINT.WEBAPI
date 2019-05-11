using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public interface IClaims
    {
        List<Claim> GetClaims(IApiUser apiUser);
    }
}
