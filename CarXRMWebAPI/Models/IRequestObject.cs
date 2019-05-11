using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public interface IRequestObject
    {
        string Vin { get; set; }
        List<object> ObjectList { get; set; }
    }
}
