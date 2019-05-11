using CarXMRWebAPI;
using System.Collections.Generic;

namespace CarXRMWebAPI
{
    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> GetVehiclesByDealerId(int DealerId);
        long AddVehicles(IEnumerable<Vehicle> vehicles);
    }
}
