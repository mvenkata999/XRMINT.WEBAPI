using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public interface IApiUser
    {
        int UserId { get; set; }
        string UserName { get; set; }
        List<Role> RoleList { get; set; }
        List<Dealer> DealerList { get; set; }
    }
}
