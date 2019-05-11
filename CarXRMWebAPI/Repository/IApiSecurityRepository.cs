using CarXRMWebAPI.Models;
using CarXRMWebAPI.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Repository
{
    public interface IApiSecurityRepository
    {
        bool CheckUserDealerIds(Claims claims, PermissionSet permissionSet, Permission permission);
        bool CheckUserPermission(string UserName, PermissionSet permissionSet, Permission permission);
    }
}
