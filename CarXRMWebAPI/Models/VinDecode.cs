using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class VinDecode
    {
        private string connectionStringChrome;
        public VinDecode()
        {
            connectionStringChrome = @"Server=HouSql02Prd;Database=Chrome;Persist Security Info=True;User ID=sa;password=whitehouse";
        }

        public string vin { get; set; }
        public string vin8 { get; set; }


        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionStringChrome);
            }
        }

        public IEnumerable<DecodedVehicle> GetLast8Lookup(string vin)
        {
            DataTable v8 = new DataTable();
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = "SELECT * FROM dbo.UDF_CRM_VINDECODE_CHROME_GENERIC_TOPROW('" + vin + "')";
                //dbConnection.Open();
                //v8 = dbConnection.Execute(sQuery);
                dbConnection.Open();
                return dbConnection.Query<DecodedVehicle>("SELECT * FROM dbo.UDF_CRM_VINDECODE_CHROME_GENERIC_TOPROW('" + vin + "')");

            }
        }
    }    
}
