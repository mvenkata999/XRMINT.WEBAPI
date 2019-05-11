using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace CarXRMWebAPI
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticateMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var k = context.Request.Headers.Keys;

            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                //Extract credentials
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);

                string path = context.Request.Path;
                char[] delimiterChars = { ' ', ',', '.', ':', '\t', '/' };
                string[] parts = path.Split(delimiterChars);

                List<string> pathParts = new List<string>();
                pathParts = parts.ToList();

                VendorsRepository vr = new VendorsRepository();

                switch (pathParts[2].ToString())
                {
                    case "DickHannah":
                        VendorAccess UserCreds = vr.GetByID(1009);
                        if (username == UserCreds.VendorUsername && password == UserCreds.VendorPassword)
                        {
                            await _next.Invoke(context);
                        }
                        else
                        {
                            context.Response.StatusCode = 401; //Unauthorized
                            return;
                        }
                        break;
                    case "values":
                        if (username == "test" && password == "test")
                        {
                            await _next.Invoke(context);
                        }
                        else
                        {
                            context.Response.StatusCode = 401; //Unauthorized
                            return;
                        }
                        break;
                    case "VinExplode":
                        if (username == "test" && password == "test")
                        {
                            await _next.Invoke(context);
                        }
                        else
                        {
                            context.Response.StatusCode = 401; //Unauthorized
                            return;
                        }
                        break;
                    case "VinDecode":
                        if (username == "test" && password == "test")
                        {
                            await _next.Invoke(context);
                        }
                        else
                        {
                            context.Response.StatusCode = 401; //Unauthorized
                            return;
                        }
                        break;

                    default:
                        context.Response.StatusCode = 401; //Unauthorized
                        return;
                      
                }
            }
            else
            {
                // no authorization header
                // context.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"realm\"");            //Adds login window to startup page
                context.Response.StatusCode = 401; //Unauthorized
                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticateMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticateMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticateMiddleware>();
        }
        public static IApplicationBuilder UseProcessingTimeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ProcessingTimeMiddleware>();
        }
    }
}
