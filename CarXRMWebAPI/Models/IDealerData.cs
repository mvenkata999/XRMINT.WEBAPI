using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public interface IDealerData
    {
        int DealerId { get; set; }
       IEnumerable<ICustomer> CustomerList { get; set; }
    }
}
