using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class ApiUser : IApiUser
    {
        private int _userId = -1;
        private string _userName = string.Empty;        
        private List<Role> _roleList = null;
        private List<Dealer> _dealerList = null;

        public ApiUser()
        {
            
        }

        public int UserId { get => _userId; set => _userId = value; }
        public string UserName { get => _userName; set => _userName = value; }        
        public List<Role> RoleList { get => _roleList; set => _roleList = value; }
        public List<Dealer> DealerList { get => _dealerList; set => _dealerList = value; }
        
    }

    public class User
    {
        private int _userId = -1;
        private string _userName = string.Empty;

        public int UserId { get => _userId; set => _userId = value; }
        public string UserName { get => _userName; set => _userName = value; }
    }

    public class Role
    {
        private int _roleId = -1;
        private string _roleName = string.Empty;

        public int RoleId { get => _roleId; set => _roleId = value; }
        public string RoleName { get => _roleName; set => _roleName = value; }
    }

    public class Dealer
    {
        private int _dealerId = -1;
        private string _dealerName = string.Empty;

        public int DealerId { get => _dealerId; set => _dealerId = value; }
        public string DealerName { get => _dealerName; set => _dealerName = value; }
    }
}
