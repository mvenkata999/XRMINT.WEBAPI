using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class CustomerRequest : ICustomerRequest
    {
        private List<int> _dealerIds = null;
        private string _roleIds = string.Empty;
        private int _userId = -1;

        public List<int> DealerIds { get => _dealerIds; set => _dealerIds = value; }
        public string RoleIds { get => _roleIds; set => _roleIds = value; }
        public int UserId { get => _userId; set => _userId = value; }
    }
}
