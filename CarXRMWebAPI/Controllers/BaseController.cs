using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarXRMWebAPI.Models.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarXRMWebAPI.Controllers
{
    public class BaseController : Controller
    {
        #region Protected Properties

        protected ClaimsUser LoggedInUser => new ClaimsUser(User);

        #endregion Protected Properties
    }
}