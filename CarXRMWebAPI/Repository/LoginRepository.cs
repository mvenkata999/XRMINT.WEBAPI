using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CarXRMWebAPI.Models;
using Dapper;

namespace CarXRMWebAPI.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private string connectionString;
        private Utilities _utilities;

        public LoginRepository()
        {
            _utilities = new Utilities();
        }
        public IApiUser CheckUser(string UserName, string Password)
        { 
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    try
                    {
                        var param = new DynamicParameters();                        
                        param.Add("@UserName", UserName, dbType: DbType.String);
                        param.Add("@Password", Password, dbType: DbType.String);
                        using (var multiResult = conn.QueryMultiple("P_XRMAPI_LOGIN", param, commandType: CommandType.StoredProcedure))
                        {
                            var userObj = multiResult.Read<User>().First();                            
                            var dealerObj = multiResult.Read<Dealer>().ToList();
                            var roleObj = multiResult.Read<Role>().ToList();

                            ApiUser apiUserObj = new ApiUser();
                            apiUserObj.UserId = userObj.UserId;
                            apiUserObj.UserName = userObj.UserName;
                            apiUserObj.DealerList = dealerObj;
                            apiUserObj.RoleList = roleObj;

                            return apiUserObj;
                        }
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

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
        private SqlConnection GetConnection()
        {
            connectionString = @"Server=HOUSQL02PRD;Database=XRMAPISecurity;Persist Security Info=True;User ID=sa;password=whitehouse";
            SqlConnection connection = new SqlConnection(connectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }




    }
}
