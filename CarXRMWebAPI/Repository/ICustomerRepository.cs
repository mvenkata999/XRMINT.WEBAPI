using CarXRMWebAPI.Models;
using CarXRMWebAPI.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Repository
{
    public interface ICustomerRepository 
    {   
        bool HasPermission(string dealerIds, CustomerRequest custRequest, PermissionSet permissionSet, Permission permission);
        IEnumerable<IDealerData> GetCustomerList(string requestedDealerIds, CustomerRequest custRequest, PermissionSet permissionSet, Permission permission);
    }
}
