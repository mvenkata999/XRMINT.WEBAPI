using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Common;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CarXRMWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class DickHannahController : Controller
    {
        private readonly VendorsRepository vendorsRepository;

        public DickHannahController()
        {
            vendorsRepository = new VendorsRepository();
        }

        // GET: api/DickHannah
        [HttpGet]
        public IEnumerable<VendorAccess> Get()
        {
             //return new string[] { "value1", "value2" };
            return vendorsRepository.GetAll();            
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public VendorAccess Get(int id)
        {
            // return vendorsRepository.GetByID(id);
            return null;
        }

        //[HttpGet("Reports")]
        //public JsonResult Reports()
        //{
        //    return Json(_authorRepository.List());
        //}

        // GET api/DickHannah/About
        [HttpGet("About")]
        public ContentResult About()
        {
            return Content("An API listing authors of docs.asp.net.");
        }

        [HttpGet("Reports2")]
        public DbDataReader Reports2(DickHannahInputObject obj)
        {
            return vendorsRepository.GetReports("xxx", null, System.Data.CommandType.StoredProcedure);
        }

        // POST api/values
        [HttpPost("Reports3")]
        public DbDataReader Reports3(DickHannahInputObject obj)
        {
            return vendorsRepository.GetReports("xxx", null, System.Data.CommandType.StoredProcedure);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]VendorAccess vendor)
        {
            //if (ModelState.IsValid)
            //    vendorsRepository.Add(vendor);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]VendorAccess vendor)
        {
            //vendor.VendorID = id;
            //if (ModelState.IsValid)
            //    vendorsRepository.Update(vendor);

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           // vendorsRepository.Delete(id);
        }
    }


}
