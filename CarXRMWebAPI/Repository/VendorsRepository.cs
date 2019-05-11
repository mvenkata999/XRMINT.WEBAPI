using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace CarXRMWebAPI
{
    public class VendorsRepository
    {
        private string connectionString;
        public VendorsRepository()
        {
            connectionString = @"Server=CarSqlServer;Database=CARDB;Persist Security Info=True;User ID=sa;password=whitehouse";
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public void Add(VendorAccess vendors)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO tblVendorsAPI (VendorName, VendorUsername, VendorPassword,Active)"
                                + " VALUES(@Name, @VendorUsername, @VendorPassword,1)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, vendors);
            }
        }

        public IEnumerable<VendorAccess> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<VendorAccess>("SELECT * FROM tblVendorsAPI");
            }
        }

        private SqlConnection GetConnection(string dbName)
        {
            string connstr = this.connectionString;
            string newConnstr = connstr.Replace("CARDB", "DickHannah");
            SqlConnection connection = new SqlConnection(newConnstr);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

        protected DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);
            command.CommandType = commandType;
            return command;
        }
        public DbDataReader GetReports(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {

            DbDataReader dr;

            try
            {
                DbConnection connection = this.GetConnection("DickHannah");
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to GetDataReader for " + procedureName, ex, parameters);
                throw;
            }

            return dr;

        }

        public VendorAccess GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM tblVendorsAPI"
                               + " WHERE VendorID = " + id;
                dbConnection.Open();
                return dbConnection.Query<VendorAccess>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public List<Int32> GetByDealerGroup(string GroupID)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT DealershipID FROM CARDB.dbo.tblDealerships_DealerGroups"
                               + " WHERE DealershipGroupID = '" + GroupID + "'";
                dbConnection.Open();
                return dbConnection.Query<Int32>(sQuery).ToList();
            }
        }
    }
}
