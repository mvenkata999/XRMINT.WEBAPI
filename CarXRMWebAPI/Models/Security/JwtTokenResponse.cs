using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models.Security
{
    public class JwtTokenResponse : IJwtTokenResponse
    {
        private string _access_token = string.Empty;
        private int _expires_in = -1;

        public string access_token { get => _access_token; set => _access_token = value; }
        public int expires_in { get => _expires_in; set => _expires_in = value; }
    }
}
