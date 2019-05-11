using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class ApiUserProfile : IApiUserProfile
    {
        private int _profileId = -1;
        private string _profileName = string.Empty;
        private string _profileDesc = string.Empty;

        public int ProfileId { get => _profileId; set => _profileId = value; }
        public string ProfileName { get => _profileName; set => _profileName = value; }
        public string ProfileDesc { get => _profileDesc; set => _profileDesc = value; }
    }
}
