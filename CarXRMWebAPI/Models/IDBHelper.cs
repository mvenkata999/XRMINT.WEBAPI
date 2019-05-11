using System.Data;

namespace CarXRMWebAPI.Models
{
    public interface IDBHelper
    {
        string DBServer { get; set; }
        string DBName { get; set; }
        string DBUserId { get; set; }
        string DBPassword { get; set; }
        IDbConnection GetConnection();
    }
}
