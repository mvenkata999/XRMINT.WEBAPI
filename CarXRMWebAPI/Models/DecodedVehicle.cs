using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class DecodedVehicle
    {
        public string vin { get; set; }
        public string ModelYear { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string StyleName { get; set; }
        public string BodyType { get; set; }
        public string BodyTypeFull { get; set; }
        public string CarTruck { get; set; }
        public string UvcCode { get; set; }
        public string ChromeAcode { get; set; }
        public string FuelType { get; set; }
        public string CarrModelCode { get; set; }
    }
}
