using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class Customer : ICustomer
    {  
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EntryDate { get; set; }
        public string Home_Number { get; set; }
        public string Work_Number { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int DealershipID { get; set; }
        public string CustStatus { get; set; }
        public string CustType { get; set; }
        public string Cell_Number { get; set; }
        public string Email { get; set; }
        public string WorkEmail { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
    }
}
