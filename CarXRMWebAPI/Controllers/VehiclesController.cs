using Microsoft.AspNetCore.Mvc;
using System;

namespace CarXRMWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class VehiclesController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;        

        public VehiclesController()
        {
            _vehicleRepository = new VehicleRepository();
        }

        [HttpGet("About")]
        public IActionResult About()
        {
            try
            {
                string strAbout = "An API listing authors of docs.asp.net.";
                return new JsonResult(strAbout);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Unexpected error has occurred. Please try again after sometime.");
            }
        }

        // GET api/Vehicles/{id}
        [HttpGet("{id}")]

        public IActionResult Vehicles(int id)
        {
            try
            {
                return new JsonResult(_vehicleRepository.GetVehiclesByDealerId(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Unexpected error has occurred. Please try again after sometime.");
            }
        }        
    }
}