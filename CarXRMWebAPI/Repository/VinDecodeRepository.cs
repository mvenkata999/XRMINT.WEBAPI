using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarXRMWebAPI.Models;

namespace CarXRMWebAPI.Repository
{
    public abstract class VinDecodeRepository : IVinDecodeRepository
    {
        public abstract bool CheckVINExists(string vin8, string modelYearCode);
        public abstract IEnumerable<DecodedVehicle> GetVinData(string vin);

        public abstract Task<IResponseObject> ThirdPartyVinDecode(string vin);

        public abstract IResponseObject SaveVinDecode(IRequestObject reqObj);
    }
}
