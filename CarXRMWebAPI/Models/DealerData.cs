using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class DealerData : IDealerData
    {
        private int _dealerId = -1;
        private IEnumerable<ICustomer> _customerList = null;

        public DealerData()
        {   
        }
        public int DealerId { get => _dealerId; set => _dealerId = value; }
        public IEnumerable<ICustomer> CustomerList { get => _customerList; set => _customerList = value; }
    }
}
