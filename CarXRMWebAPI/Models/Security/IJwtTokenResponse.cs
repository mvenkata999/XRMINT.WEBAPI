using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models.Security
{
    public interface IJwtTokenResponse
    {
        string access_token { get;set;}
        int expires_in { get;set;}
    }
}
