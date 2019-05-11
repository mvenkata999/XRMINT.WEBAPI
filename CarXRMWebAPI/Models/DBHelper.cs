using System;
using System.Data;
using System.Data.SqlClient;

namespace CarXRMWebAPI.Models
{
    public class DBHelper : IDBHelper
    {
        public DBHelper()
        {   
        }
        private string _dbServer;
        public string DBServer
        {
            get
            {
                return _dbServer;
            }
            set { _dbServer = value; }
        }

        private string _dbName;
        public string DBName
        {
            get
            {
                return _dbName;
            }
            set { _dbName = value; }
        }

        private string _dbUserId;
        public string DBUserId
        {
            get
            {
                return _dbUserId;
            }
            set { _dbUserId = value; }
        }

        private string _dbPassword;
        public string DBPassword
        {
            get
            {
                return _dbPassword;
            }
            set { _dbPassword = value; }
        }

        public IDbConnection GetConnection()
        {
            try
            {
                SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder
                {
                    DataSource = DBServer,
                    InitialCatalog = DBName,
                    UserID = DBUserId,
                    Password = DBPassword
                };

                return new SqlConnection(sb.ConnectionString);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
