using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Helpers
{
    public interface IAppSettings
    {
        string GetAppSetting(string keyName);
    }
}
