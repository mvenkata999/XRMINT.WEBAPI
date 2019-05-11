using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public interface ICustomer 
    {
        int CustomerId { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string EntryDate { get; set; }
        string Home_Number { get; set; }
        string Work_Number { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }
        int DealershipID { get; set; }
        string CustStatus { get; set; }
        string CustType { get; set; }
        string Cell_Number { get; set; }
        string Email { get; set; }
        string WorkEmail { get; set; }
        string Gender { get; set; }
        string Ethnicity { get; set; }
    }
}
