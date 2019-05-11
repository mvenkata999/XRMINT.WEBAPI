using CarXRMWebAPI.Models;
using CarXRMWebAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Controllers
{    
    //[Authorize(Policy = "AuthorizationPolicy")]
    [Produces("application/json")]
    [Route("api/VinDecode")]
    [EnableCors("CorsPolicy")]
    public class VinDecodeController : Controller
    {
        private readonly IVinDecodeRepository _VinDecodeRepository;

        public VinDecodeController()
        {
            _VinDecodeRepository = new ChromeVinDecodeRepository();
        }

        // GET api/VinDecode/{id}
        [HttpGet("{vin}/{thirdpartyflag}")]
        public async Task<IActionResult> VinDecode(string vin, bool thirdPartyFlag)
        {
            try
            {
                IResponseObject respObj = null;
                JsonResult jsonResultObj = null;
                IEnumerable<DecodedVehicle> decodedVehicle = null;

                string vin8 = string.Empty;
                string yearCode = string.Empty;
                if (vin.Length != 17)
                {
                    respObj = new ResponseObject();
                    respObj.ErrId = 1001;
                    respObj.ErrDesc = "A valid 17 digit VIN is a required parameter to decode VIN.";
                    jsonResultObj = new JsonResult(respObj);
                    return jsonResultObj;
                }
                else
                {
                    vin8 = vin.Substring(0, 8);
                    yearCode = vin.Substring(9, 1);
                }

                bool bVinExists = _VinDecodeRepository.CheckVINExists(vin8, yearCode);                
                if (!thirdPartyFlag && bVinExists)
                {
                    _VinDecodeRepository.GetVinData(vin);
                }
                else
                {
                    respObj = await _VinDecodeRepository.ThirdPartyVinDecode(vin);
                    if (respObj.ErrId !=0)
                    {
                        respObj.ErrDesc = "Not able to decode VIN at this time. Please try again at a later time.";
                    }
                    else
                    {   
                        IRequestObject reqObj = new RequestObject();
                        reqObj.ObjectList = respObj.ObjectList;
                        respObj = _VinDecodeRepository.SaveVinDecode(reqObj);
                        if (respObj != null)
                        {
                            if (respObj.ErrId == 0)
                            {
                                decodedVehicle = _VinDecodeRepository.GetVinData(vin);
                                if (decodedVehicle != null)
                                {
                                    respObj.ObjectList.Add(decodedVehicle);
                                }
                            }                            
                        }
                        else
                        {
                            //error handling
                            respObj = new ResponseObject();
                            respObj.ErrId = 1006;
                            respObj.ErrDesc = "No valid response is received saving vin data";
                        }
                    }
                }

                jsonResultObj = new JsonResult(respObj);
                return jsonResultObj;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Unexpected error has occurred. Please try again after sometime.");
            }
        }

        [HttpPost("{vin}")]
        public async Task<IActionResult> ThirdPartyVinDecode(string vin)
        {
            try
            {
                IResponseObject respObj = null;
                JsonResult jsonResultObj = null;
                respObj = await _VinDecodeRepository.ThirdPartyVinDecode(vin);
                jsonResultObj = new JsonResult(respObj);

                return jsonResultObj;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Unexpected error has occurred. Please try again after sometime.");
            }
        }
    }
}