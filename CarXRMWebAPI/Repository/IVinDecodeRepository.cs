using CarXRMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Repository
{
    public interface IVinDecodeRepository
    {
        bool CheckVINExists(string vin8, string modelYearCode);
        IEnumerable<DecodedVehicle> GetVinData(string vin);

        Task<IResponseObject> ThirdPartyVinDecode(string vin);
        IResponseObject SaveVinDecode(IRequestObject reqObj);
    }
}
