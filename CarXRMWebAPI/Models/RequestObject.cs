using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class RequestObject : IRequestObject
    {
        private string _vin = string.Empty;
        private List<object> _ObjectList = null;

        public RequestObject()
        {
            ObjectList = new List<object>();
        }

        public string Vin { get => _vin; set => _vin = value; }
        public List<object> ObjectList { get => _ObjectList; set => _ObjectList = value; }
    }
}
