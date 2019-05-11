using AutoMapper;
using CarXMRWebAPI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CarXRMWebAPI.Controllers
{
    [Route("api/VehicleCollection")]
    public class VehicleCollectionController : Controller
    {
        private readonly VehicleRepository _vehicleRepository;

        public VehicleCollectionController()
        {
            _vehicleRepository = new VehicleRepository();
        }

        [HttpPost]
        public IActionResult CreateVehicleCollection([FromBody]IEnumerable<Vehicle> vehicles)
        {
            try
            {
                if (vehicles == null)
                {
                    return BadRequest();
                }
                
                return new JsonResult(_vehicleRepository.AddVehicles(vehicles));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Unexpected error has occurred. Please try again after sometime.");
            }
        }
    }
}