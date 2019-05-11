using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace CarXRMWebAPI.Models.Attributes
{
    public class XRMCacheApiAttribute : ActionFilterAttribute
    {
        public int Duration { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            filterContext.Response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(Duration),
                MustRevalidate = true,
                Private = true
            };
        }
    }
}
