using System;
using System.Reflection;
using System.Resources;

namespace CarXRMWebAPI
{
    public class Utilities
    {
        public string ReadResourceValue(string resourceKey, string resourceFile = "CarXRMWebAPI.Resource")
        {
            try
            {
                ResourceManager resourceManager = new ResourceManager(resourceFile, Assembly.GetEntryAssembly());
                return resourceManager.GetString(resourceKey);
            }
            catch (Exception ex)
            {   
                throw;
            }
        }
    }
}
