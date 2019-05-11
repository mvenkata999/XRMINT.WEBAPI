using CarXMRWebAPI;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace CarXRMWebAPI
{
    public class VehicleRepository : IVehicleRepository
    {
        private string connectionString;
        private Utilities _utilities;
        public VehicleRepository()
        {
            _utilities = new Utilities();
            connectionString = @"Server=CarSqlServer;Database=CARDB;Persist Security Info=True;User ID=sa;password=whitehouse";
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
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

        public IEnumerable<Vehicle> GetVehiclesByDealerId(int DealerId)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string sQuery = "SELECT * FROM INVENTORYDATA.DBO.tblVehicleDetails"
                                    + " WHERE DealerID = " + DealerId;

                    dbConnection.Open();
                    return dbConnection.Query<Vehicle>(sQuery);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public long GetVehicleMaxDetailId()
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string sQuery = "SELECT DetailId FROM INVENTORYDATA.DBO.tblVehicleDetails";

                    dbConnection.Open();
                    return dbConnection.Query<Int32>(sQuery).Max();                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public long AddVehicles(IEnumerable<Vehicle> vehicles)
        {
            try
            {   
                using (IDbConnection dbConnection = Connection)
                {
                    if (null != vehicles)
                    {
                        long maxDetailId = GetVehicleMaxDetailId();
                        if (maxDetailId > 0)
                        {
                            foreach (var vehicle in vehicles)
                            {
                                vehicle.DetailID = (++maxDetailId).ToString();                                
                            }
                        }
                    }

                    string strInsertVehDetailQuery = _utilities.ReadResourceValue("InsertVehicleDetailQuery");
                    dbConnection.Open();
                    return dbConnection.Execute(strInsertVehDetailQuery, vehicles);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
