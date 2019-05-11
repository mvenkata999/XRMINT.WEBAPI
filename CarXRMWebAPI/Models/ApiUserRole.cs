using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class ApiUserRole : IApiUserRole
    {
        private int _roleId = -1;
        private string _roleName = string.Empty;
        private string _roleDesc = string.Empty;

        public int RoleId { get => _roleId; set => _roleId = value; }
        public string RoleName { get => _roleName; set => _roleName = value; }
        public string RoleDesc { get => _roleDesc; set => _roleDesc = value; }
    }
}
