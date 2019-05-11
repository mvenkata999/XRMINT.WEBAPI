using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class DealerInfo : IDealerInfo
    {
        private int _dealershipId = -1;
        private string _dealershipName = string.Empty;
        //private int _defaultDealershipId = 0;
        private string _dbServer = string.Empty;
        private string _dbName = string.Empty;

        public int DealershipId { get => _dealershipId; set => _dealershipId = value; }
        public string DealershipName { get => _dealershipName; set => _dealershipName = value; }
        public string DbServer { get => _dbServer; set => _dbServer = value; }
        public string DbName { get => _dbName; set => _dbName = value; }

        //public int DefaultDealershipId { get => _defaultDealershipId; set => _defaultDealershipId = value; }
    }
}
