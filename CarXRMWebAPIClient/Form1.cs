using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace CarXRMWebAPIClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void bttnGetCustomerList_Click(object sender, EventArgs e)
        {
            try
            {   
                string userName = "hondasales";
                string password = "hondasales123!";
                string accessToken = string.Empty;
                Request reqObject = new Request(userName, password);
                string url = "http://webapi.car-research.com/CarXRMWebAPI/api/1.0/Security/JWT/Login";
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.RequestFormat = DataFormat.Json;
                var encodedBody = JsonConvert.SerializeObject(reqObject);
                request.AddParameter("application/json", encodedBody, ParameterType.RequestBody);                
                IRestResponse response = client.Execute(request);                
                RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();

                string resp = response.Content.Replace("\\n", "");
                resp = resp.Replace("\\r", "");
                resp = resp.Replace(";", "");
                resp = resp.Remove(resp.IndexOf("\""), 1);
                if (resp.LastIndexOf("\"") != -1)
                    resp = resp.Remove(resp.LastIndexOf("\""));

                resp = resp.Replace("\\ ", "");
                resp = resp.Replace("\\t", "");
                resp = resp.Replace(@"\", "");

                var list = JsonConvert.DeserializeObject<Response>(resp);

                //get customer list
                CustomerRequest custReq = new CustomerRequest();
                custReq.DealerIds.Add(285);
                custReq.DealerIds.Add(292);
                string url1 = "http://webapi.car-research.com/CarXRMWebAPI/api/1.0/Sales/Customer/GetCustomerList";
                var client1 = new RestClient(url1);
                var request1 = new RestRequest(Method.POST);
                //request1.AddHeader("cache-control", "no-cache");
                request1.AddHeader("content-type", "application/json");
                request1.AddHeader("Authorization", "Bearer " + list.access_token);
                request1.RequestFormat = DataFormat.Json;
                var encodedBody1 = JsonConvert.SerializeObject(custReq);
                request1.AddParameter("application/json", encodedBody1, ParameterType.RequestBody);

                IRestResponse response1 = client1.Execute(request1);
                var custList = JsonConvert.DeserializeObject<List<DealerCustomers>>(response1.Content);
                if (custList != null)
                {
                    richTxtBxResp.Text = JsonConvert.SerializeObject(custList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class Request
    {
        private string _userName = string.Empty;
        private string _password = string.Empty;

        public string userName { get => _userName; set => _userName = value; }
        public string password { get => _password; set => _password = value; }

        public Request(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }
    }

    public class Response
    {
        private string _access_token = string.Empty;
        private int _expires_in = 0;
        //private int _other_data = 0;

        public string access_token { get => _access_token; set => _access_token = value; }
        public int expires_in { get => _expires_in; set => _expires_in = value; }
        //public int other_data { get => _other_data; set => _other_data = value; }
    }

    public class CustomerRequest
    {
        public List<int> DealerIds { get; set; }

        public CustomerRequest()
        {
            DealerIds = new List<int>();
        }

    }

    public class DealerCustomers
    {   
        public int DealerId { get; set; }
        public IEnumerable<Customer> CustomerList { get; set; }
    }
    public class Customer
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
