using CarXRMWebAPI.Controllers;
using CarXRMWebAPI.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace CarXRMWebAPI.Models.Filters
{
    public class ApiAuthenticationFilter : GenericAuthenticationFilter
    {
        /// <summary>
        /// Default Authentication Constructor
        /// </summary>
        public ApiAuthenticationFilter()
        {
        }

        /// <summary>
        /// AuthenticationFilter constructor with isActive parameter
        /// </summary>
        /// <param name="isActive"></param>
        public ApiAuthenticationFilter(bool isActive)
            : base(isActive)
        {
        }

        /// <summary>
        /// Protected overriden method for authorizing user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            var provider = actionContext.ControllerContext.Configuration
                               .DependencyResolver.GetService(typeof(JWTController)) as JWTController;
            if (provider != null)
            {
                LoginUser loginUser = new LoginUser();
                loginUser.UserName = username;
                loginUser.Password = password;
                var objResult  = provider.Login(loginUser);
                if (objResult != null)
                {
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    if (basicAuthenticationIdentity != null)
                        //basicAuthenticationIdentity.UserId = userId;
                    return true;
                }
            }
            return false;
        }
    }

}

