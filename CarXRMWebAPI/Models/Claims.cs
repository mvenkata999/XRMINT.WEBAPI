using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class Claims : IClaims
    {   
        private List<Claim> _claimList;
        public Claims()
        {
            ClaimList = new List<Claim>();
        }

        public List<Claim> ClaimList { get => _claimList; set => _claimList = value; }

        public List<Claim> GetClaims(IApiUser apiUser)
        {
            try
            {
                List<Claim> claimList = new List<Claim>();                
                //claimList.Add(new Claim("Role", apiUser.Role));
                //claimList.Add(new Claim("Profile", apiUser.Profile));

                return claimList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
