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
    public class JwtToken : IJwtToken
    {
        private IJwtFactory _jwtFactory = null;
        private IJwtTokenResponse _jwtTokenResponse = null;
        public JwtToken(IJwtFactory jwtFactory)
        {
            _jwtFactory = jwtFactory;
            _jwtTokenResponse = new JwtTokenResponse();
        }
        public async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            //var response = new
            //{
            _jwtTokenResponse.access_token = await _jwtFactory.GenerateEncodedToken(userName, identity);
            _jwtTokenResponse.expires_in = (int)jwtOptions.ValidFor.TotalSeconds;


                //other_data = 555
            //};

            return JsonConvert.SerializeObject(_jwtTokenResponse, serializerSettings);
        }
    }
}
