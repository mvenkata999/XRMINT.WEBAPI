using CarXRMWebAPI.Models;
using CarXRMWebAPI.Models.Attributes;
using CarXRMWebAPI.Models.Security;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CarXRMWebAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {   
        private IDBHelper _dbHelper = null;

        public CustomerRepository(IDBHelper dbHelper)
        {   
            _dbHelper = dbHelper;
        }

        public bool HasPermission(string dealerIds, CustomerRequest custRequest, PermissionSet permissionSet, Permission permission)
        {
            try
            {
                if (custRequest == null)
                    return false;
                
                int UserId = custRequest.UserId;
                bool bRetCd = false;
                _dbHelper.DBServer = "HOUSQL02PRD";
                _dbHelper.DBName = "XRMAPISecurity";
                _dbHelper.DBUserId = "sa";
                _dbHelper.DBPassword = "whitehouse";

                using (IDbConnection conn = _dbHelper.GetConnection())
                {
                    try
                    {
                        string permSetName = Enum.GetName(typeof(PermissionSet), permissionSet);
                        string permName = Enum.GetName(typeof(Permission), permission);

                        var param = new DynamicParameters();
                        param.Add("@UserId", UserId, dbType: DbType.Int32);
                        param.Add("@DealerIdList", dealerIds, dbType: DbType.String);
                        param.Add("@RoleIdList", custRequest.RoleIds, dbType: DbType.String);
                        param.Add("@PermissionSet", permSetName, dbType: DbType.String);
                        param.Add("@Permission", permName, dbType: DbType.String);
                        param.Add("@RetVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                        conn.Execute("dbo.P_XRMAPI_Validate_UserRequest", param, commandType: CommandType.StoredProcedure);

                        if (param.Get<Int32>("@RetVal") == 1)
                        {   
                            bRetCd = true;
                        }

                        conn.Close();
                        return bRetCd;
                    }
                    catch (ArgumentNullException)
                    {
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public IEnumerable<IDealerData> GetCustomerList(string requestedDealerIds, CustomerRequest custRequest, PermissionSet permissionSet, Permission permission)
        {
            try
            {   
                List<IDealerData> dealerDataList = new List<IDealerData>();
                //List<IEnumerable<ICustomer>> customerList = new List<IEnumerable<ICustomer>>();
               
                IEnumerable<IDealerInfo> dealerInfoList = GetDealerDetails(requestedDealerIds, custRequest);
                if (dealerInfoList == null)
                    return null;
                
                foreach (DealerInfo dealerInfo in dealerInfoList)
                {
                    IDealerData dealerData = new DealerData();
                    IEnumerable<Fields> fields = GetCustomerFieldsByDealerId(dealerInfo, custRequest, permissionSet, permission);
                    IEnumerable<ICustomer> result = GetCustomerByDealerId(fields, dealerInfo);                    
                    dealerData.DealerId = dealerInfo.DealershipId;
                    dealerData.CustomerList = result;
                    dealerDataList.Add(dealerData);
                }
                
                return dealerDataList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private IEnumerable<IDealerInfo> GetDealerDetails(string requestedDealerIds, CustomerRequest custRequest)
        {
            try
            {
                _dbHelper.DBServer = "CARSQLServer";
                _dbHelper.DBName = "CARDB";
                _dbHelper.DBUserId = "sa";
                _dbHelper.DBPassword = "whitehouse";

                using (IDbConnection dbConnection = _dbHelper.GetConnection())
                {
                    string sQuery = "SELECT d.DealershipId DealershipId, d.DealershipTranslation DealershipName, ISNULL(d.ServerName,'CarSqlServer') DbServer,  d.DbName DbName "
                    + " FROM Cardb.dbo.tblDealerships d WITH (READUNCOMMITTED)"
                    + " WHERE d.DealershipId IN (" + requestedDealerIds + ")"
                    + " AND ISNULL(d.crmInactive,0) = 0 ";                

                    dbConnection.Open();
                    var dealerInfoObj = dbConnection.Query<DealerInfo>(sQuery);
                    return dealerInfoObj;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private IEnumerable<Fields> GetCustomerFieldsByDealerId(IDealerInfo dealerInfo, CustomerRequest custRequest, PermissionSet permissionSet, Permission permission)
        {
            try
            {
                int UserId = custRequest.UserId;                
                _dbHelper.DBServer = "HOUSQL02PRD";
                _dbHelper.DBName = "XRMAPISecurity";
                _dbHelper.DBUserId = "sa";
                _dbHelper.DBPassword = "whitehouse";

                using (IDbConnection conn = _dbHelper.GetConnection())
                {
                    try
                    {
                        string permSetName = Enum.GetName(typeof(PermissionSet), permissionSet);
                        string permName = Enum.GetName(typeof(Permission), permission);

                        var param = new DynamicParameters();
                        param.Add("@UserId", UserId, dbType: DbType.Int32);
                        param.Add("@DealerId", dealerInfo.DealershipId, dbType: DbType.String);
                        param.Add("@RoleIdList", custRequest.RoleIds, dbType: DbType.String);
                        param.Add("@PermissionSet", permSetName, dbType: DbType.String);
                        param.Add("@Permission", permName, dbType: DbType.String);

                        return conn.Query<Fields>("P_XRMAPI_GetFields", param, commandType: CommandType.StoredProcedure);
                        
                    }
                    catch (ArgumentNullException)
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private IEnumerable<ICustomer> GetCustomerByDealerId(IEnumerable<Fields> fields, IDealerInfo dealerInfo)
        {
            try
            {
                _dbHelper.DBServer = dealerInfo.DbServer;
                _dbHelper.DBName = dealerInfo.DbName;
                _dbHelper.DBUserId = "sa";
                _dbHelper.DBPassword = "whitehouse";
                string fieldNames = string.Empty;
                foreach (Fields field in fields)
                {
                    fieldNames += field.FieldName + ",";
                }

                if (fieldNames.LastIndexOf(',') == fieldNames.Length - 1)
                    fieldNames = fieldNames.Remove(fieldNames.LastIndexOf(','));

                using (IDbConnection dbConnection = _dbHelper.GetConnection())
                {
                    string sQuery = "SELECT TOP 10 " + fieldNames
                    + " FROM dbo.tblCustomers tc WITH (READUNCOMMITTED)"
                    + " WHERE tc.DealershipId = " + dealerInfo.DealershipId;

                    dbConnection.Open();
                    var customerObj = dbConnection.Query<Customer>(sQuery);
                    return customerObj;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
