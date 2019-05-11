using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public interface IApiUserRole
    {
        int RoleId { get; set; }
        string RoleName { get; set; }
        string RoleDesc { get; set; }
    }
}
