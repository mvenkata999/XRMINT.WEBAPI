using CarXRMWebAPI.Helpers;
using CarXRMWebAPI.Models;
using CarXRMWebAPI.Models.Attributes;
using CarXRMWebAPI.Models.Helpers;
using CarXRMWebAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarXRMWebAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Area("Sales")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}/[area]/[controller]")]    
    public class CustomerController : BaseController
    {
        private ICustomerRepository _customerRepository = null;
        private IDBHelper _dbHelper = new DBHelper();
        private readonly ILogger _logger;

        public CustomerController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<JWTController>();
            _customerRepository = new CustomerRepository(_dbHelper);        
        }
        
        [HttpPost]
        [XRMApiSecurity(PermissionSet.Customer, Permission.ReadCustomer)]
        [Route("GetCustomerList")]
        //[XRMCacheApiAttribute(Duration = 0)]
        public IActionResult GetCustomerList([FromBody]CustomerRequest custRequest)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.GetItem, "Begin GetCustomerList API", custRequest.UserId + ":" + custRequest.DealerIds + ":" + custRequest.RoleIds);
                if (custRequest == null)
                {
                    _logger.LogInformation(LoggingEvents.GetItem, "Bad Request", custRequest.UserId + ":" + custRequest.DealerIds + ":" + custRequest.RoleIds);
                    return BadRequest();
                }
             
                List<int> tokenDealerIds = LoggedInUser.DealerIds.Split(',')
                    .Select(t => int.Parse(t))
                    .ToList();

                string requestedDealerIds = string.Empty;
                foreach (int dealerId in custRequest.DealerIds)
                {
                    if (!tokenDealerIds.Contains(dealerId))
                    {
                        _logger.LogInformation(LoggingEvents.GetItem, "Unauthorized", custRequest.UserId + ":" + custRequest.DealerIds + ":" + custRequest.RoleIds);
                        return Unauthorized();
                    }

                    requestedDealerIds += dealerId + ",";
                }

                if (requestedDealerIds.LastIndexOf(',') == requestedDealerIds.Length - 1)
                    requestedDealerIds = requestedDealerIds.Remove(requestedDealerIds.LastIndexOf(','));

                custRequest.RoleIds = LoggedInUser.RoleIds;
                custRequest.UserId = LoggedInUser.UserId;

                //if (!_customerRepository.HasPermission(LoggedInUser.DealerIds, custRequest, PermissionSet.Customer, Permission.ReadCustomer))
                //    return Unauthorized();
                //else
                //{

                _logger.LogInformation(LoggingEvents.GetItem, "Begin Repository GetCustomerList", custRequest.UserId + ":" + custRequest.DealerIds + ":" + custRequest.RoleIds);
                var result = _customerRepository.GetCustomerList(requestedDealerIds, custRequest, PermissionSet.Customer, Permission.ReadCustomer);
                _logger.LogInformation(LoggingEvents.GetItem, "End Repository GetCustomerList", custRequest.UserId + ":" + custRequest.DealerIds + ":" + custRequest.RoleIds);
                if (result == null)
                    {
                        return NotFound();
                    }
                    else
                        return new ObjectResult(result);
                //}
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("GetCustomerById")]        
        public IActionResult GetCustomerById()
        {
            try
            {
                var result = 999;
                return new ObjectResult(result);                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}