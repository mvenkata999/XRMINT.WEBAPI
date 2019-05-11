using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace CarXRMWebAPI.Models.Attributes
{
    public interface IXRMAPIAuthorizeAttribute
    {
        void OnAuthorization(HttpActionContext actionContext);
    }
}
