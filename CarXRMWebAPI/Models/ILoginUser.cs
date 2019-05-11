using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public interface ILoginUser
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}
