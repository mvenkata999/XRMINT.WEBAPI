using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public interface IApiUserProfile
    {
        int ProfileId { get; set; }
        string ProfileName { get; set; }
        string ProfileDesc { get; set; }
    }
}
