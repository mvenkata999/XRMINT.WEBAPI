using System.Collections.Generic;

namespace CarXRMWebAPI.Models
{
    public interface ICustomerRequest
    {
        List<int> DealerIds { get; set; }
        string RoleIds { get; set; }
        int UserId { get; set; }
    }
}