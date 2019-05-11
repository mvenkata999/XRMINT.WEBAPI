using CarXRMWebAPI.Models;
using CarXRMWebAPI.Models.Attributes;
using Dapper;
using System;
using System.Data;
using System.Security.Claims;

namespace CarXRMWebAPI.Repository
{
    public class ApiSecurityRepository : IApiSecurityRepository
    {
        private IDBHelper _dbHelper = null;
        public ApiSecurityRepository()
        {
            _dbHelper = new DBHelper();
        }
        public bool CheckUserDealerIds(Claims claims, PermissionSet permissionSet, Permission permission)
        {
            try
            {
                bool bRetCd = false;
                _dbHelper.DBServer = "HOUSQL02PRD";
                _dbHelper.DBName = "XRMAPISecurity";
                _dbHelper.DBUserId = "sa";
                _dbHelper.DBPassword = "whitehouse";

                string dealerIds = string.Empty;
                string roleIds = string.Empty;
                string userId = string.Empty;
                foreach (Claim claim in claims.ClaimList)
                {
                    if (claim.Type == "UserId")
                    {
                        userId = claim.Value;
                    }
                    if (claim.Type == "DealerIds")
                    {
                        dealerIds = claim.Value;                        
                    }
                    if (claim.Type == "RoleIds")
                    {
                        roleIds = claim.Value;
                    }
                }
                if (dealerIds.Length == 0 || roleIds.Length == 0 || userId.Length == 0)
                {
                    return false;
                }

                using (IDbConnection conn = _dbHelper.GetConnection())
                {
                    try
                    {
                        string permSetName = Enum.GetName(typeof(PermissionSet), permissionSet);
                        string permName = Enum.GetName(typeof(Permission), permission);

                        var param = new DynamicParameters();
                        param.Add("@UserId", userId, dbType: DbType.Int32);
                        param.Add("@DealerIdList", dealerIds, dbType: DbType.String);
                        param.Add("@RoleIdList", roleIds, dbType: DbType.String);
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckUserPermission(string UserName, PermissionSet permissionSet, Permission permission)
        {
            try
            {
                bool bRetCd = false;
                _dbHelper.DBServer = "HOUSQL02PRD";
                _dbHelper.DBName = "XRMAPISecurity";
                _dbHelper.DBUserId = "sa";
                _dbHelper.DBPassword = "whitehouse";
               
                if (UserName.Length == 0)
                {
                    return false;
                }

                using (IDbConnection conn = _dbHelper.GetConnection())
                {
                    try
                    {
                        string permSetName = Enum.GetName(typeof(PermissionSet), permissionSet);
                        string permName = Enum.GetName(typeof(Permission), permission);

                        var param = new DynamicParameters();
                        param.Add("@UserName", UserName, dbType: DbType.String);                       
                        param.Add("@PermissionSet", permSetName, dbType: DbType.String);
                        param.Add("@Permission", permName, dbType: DbType.String);
                        param.Add("@RetVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                        conn.Execute("dbo.P_XRMAPI_User_Permission", param, commandType: CommandType.StoredProcedure);

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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
