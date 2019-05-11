using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace CarXRMWebAPI.Helpers
{
    public class AppSettings : IAppSettings
    {
        public string GetAppSetting(string keyName)
        {
            try
            {
                //Configuration config = ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location);                
                //AppSettingsSection adminConfigDllAppSettings = (AppSettingsSection)config.GetSection("appSettings");

                //return adminConfigDllAppSettings.Settings[keyName].Value;                
                return "";
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
