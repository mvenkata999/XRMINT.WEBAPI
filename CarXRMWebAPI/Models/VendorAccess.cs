using System;
using System.ComponentModel.DataAnnotations;

namespace CarXRMWebAPI
{
    public class VendorAccess
    {
        [Key]
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string VendorUsername { get; set; }
        public string VendorPassword { get; set; }
        public int Active { get; set; }
    }


    public class DickHannahInputObject
    {
        string processName = "";
        string dealerIds = "";
        DateTime? startDate = null;
        DateTime? endDate = null;

        public string ProcessName
        {
            get { return processName; }
            set { processName = value; }
        }

        public string DealerIds
        {
            get { return dealerIds; }
            set { dealerIds = value; }
        }

        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
    }
}
