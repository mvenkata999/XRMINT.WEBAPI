using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models.Security
{
    public class ClaimsUser : ClaimsPrincipal
    {
        #region Public Constructors

        public ClaimsUser(IPrincipal principal)
            : base(principal)
        { }

        #endregion Public Constructors

        #region Public Properties

        
        public string DealerIds => GetClaimValue<string>("DealerIds");

        public string RoleIds => GetClaimValue("RoleIds");

        public int UserId => GetClaimValue<int>("UserId");

        public string UserName => GetClaimValue(ClaimTypes.NameIdentifier);

        #endregion Public Properties

        #region Public Methods

        public Claim GetClaim(string type) => NewMethod(type);

        public string GetClaimValue(string claimType) => GetClaim(claimType)?.Value;

        public T GetClaimValue<T>(string claimType)
        {
            var claimValue = GetClaimValue(claimType)?.Trim();
            var type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }

            return (claimValue != null) ? (T)Convert.ChangeType(claimValue, type) : default;
        }

        private Claim NewMethod(string type) => Claims?.FirstOrDefault(c => c.Type == type);

        #endregion Public Methods
    }
}
