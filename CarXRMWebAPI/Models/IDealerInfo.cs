using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public interface IDealerInfo
    {
        int DealershipId { get; set; }
        string DealershipName { get; set; }
        //int DefaultDealershipId { get; set; }

        string DbServer { get; set; }
        string DbName { get; set; }

    }
}
