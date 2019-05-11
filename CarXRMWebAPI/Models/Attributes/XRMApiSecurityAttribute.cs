using CarXRMWebAPI.Models.Helpers;
using CarXRMWebAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;

namespace CarXRMWebAPI.Models.Attributes
{
    public enum PermissionSet
    {
        Customer,
        Inventory
    }

    public enum Permission
    {
        ReadCustomer,
        CreateCustomer,
        UpdateCustomer,
        DeleteCustomer,
        ReadVehicle,
        CreateVehicle,
        UpdateVehicle,
        DeleteVehicle
    }

    public class XRMApiSecurityAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly PermissionSet _permissionSet;
        private readonly Permission _permission;
        private IApiSecurityRepository _apiSecurityRepo = null;
        private ILogger _logger = null;
        private ILoggerFactory _loggerFactory = null;

        public XRMApiSecurityAttribute(PermissionSet permissionSet, Permission permission)
        {
            _loggerFactory = new LoggerFactory();
            _apiSecurityRepo = new ApiSecurityRepository();
            _permissionSet = permissionSet;
            _permission = permission;
            _logger = _loggerFactory.CreateLogger("XRMApiSecurityAttribute");
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _logger.LogInformation(LoggingEvents.AuthorizeUser, "Begin XRMAPISecurity-OnAuthorization", "");
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {   
                return;
            }

            string permSetName = Enum.GetName(typeof(PermissionSet), _permissionSet);
            string permName = Enum.GetName(typeof(Permission), _permission);

            _logger.LogInformation(LoggingEvents.AuthorizeUser, "Begin APISecurityRepository-CheckUserPermission", user.Identity.Name + ":" + permSetName + ":" + permName);
            if (!_apiSecurityRepo.CheckUserPermission(user.Identity.Name, _permissionSet, _permission))
            {
                _logger.LogInformation(LoggingEvents.AuthorizeUser, "End APISecurityRepository-CheckUserPermission Unauthorized", user.Identity.Name + ":" + permSetName + ":" + permName);
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
                return;
            }

            _logger.LogInformation(LoggingEvents.AuthorizeUser, "End APISecurityRepository-CheckUserPermission OK", user.Identity.Name + ":" + permSetName + ":" + permName);
        }
    }
}
