using CarXRMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Repository
{
    public interface ILoginRepository
    {
        IApiUser CheckUser(string UserName, string Password);
    }
}
