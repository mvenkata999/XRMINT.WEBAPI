using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public interface IResponseObject
    {
        int ErrId { get; set; }
        string ErrDesc { get; set; }
        List<object> ObjectList { get; set; }
    }
}
