using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
//using Microsoft.AspNetCore.Mvc.Filters;
using System.Web.Http.Filters;

namespace CarXRMWebAPI.Models.Attributes
{   
    public class XRMAPIAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public XRMAPIAuthorizeAttribute()
        {

        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //string username;
            //string password;

            //if (GetUserNameAndPassword(actionContext, out username, out password))
            //{
            //    username = actionContext.Request.Headers.ToString();
            //}
            //else
            //{
            //    actionContext.Response =
            //        new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            //}
        }
        private bool GetUserNameAndPassword(HttpActionContext actionContext, out string username, out string password)
        {   
            bool gotIt = false;
            username = string.Empty;
            password = string.Empty;
            IEnumerable<string> headerVals;
            if (actionContext.Request.Headers.TryGetValues("Authorization", out headerVals))
            {
                try
                {
                    string authHeader = headerVals.FirstOrDefault();
                    char[] delims = {' '};
                    string[] authHeaderTokens = authHeader.Split(new char[] {' '});
                    if (authHeaderTokens[0].Contains("Basic"))
                    {
                        string decodedStr = authHeaderTokens[1];
                        string[] unpw = decodedStr.Split(new char[] {':'});
                        username = unpw[0];
                        password = unpw[1];
                    }
                    else
                    {
                        if (authHeaderTokens.Length > 1) username = authHeaderTokens[1];                        
                    }

                    gotIt = true;
                }
                catch
                {
                    gotIt = false;
                }
            }

            return gotIt;
        }
    }
    
}
