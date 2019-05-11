using AutoFixture;
using CarXRMWebAPI.Controllers;
using CarXRMWebAPI.Models;
using CarXRMWebAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CarXRMWebAPI.UnitTests
{
    public class VinDecodeControllerTests
    {
        [Fact]
        public async void VinDecodeReturnErrorIfVINLengthLessThan17Chars()
        {
            var controller = new VinDecodeController();
            var vin = "1G1ZE5ST0GF21";
            var thirdpartyflag = false;
            IResponseObject respObj = null;
            object jsonObj = await controller.VinDecode(vin, thirdpartyflag);
            //respObj = (IResponseObject)JsonConvert.DeserializeObject(jsonObj.ToString());
            var JsonResult = Assert.IsType<JsonResult>(jsonObj);
            respObj = (IResponseObject)JsonResult.Value;
            Assert.Equal(1001, respObj.ErrId);
        }

        [Fact]
        public async void VinDecodeReturnErrorIfVINLengthGreaterThan17Chars()
        {
            var controller = new VinDecodeController();
            var vin = "1G1ZE5ST0GF2122342432222";
            var thirdpartyflag = false;
            IResponseObject respObj = null;
            object jsonObj = await controller.VinDecode(vin, thirdpartyflag);
            //respObj = (IResponseObject)JsonConvert.DeserializeObject(jsonObj.ToString());
            var JsonResult = Assert.IsType<JsonResult>(jsonObj);
            respObj = (IResponseObject)JsonResult.Value;
            Assert.Equal(1001, respObj.ErrId);
        }

        [Fact]
        public async void VinDecodeCallsChromeWebServiceIfVINNotExistsInChromeDatabase()
        {
            var vin8 = "12143241122343434";
            var modelYearCode = "2";
            var thirdPartyFlag = true;
            var mockVinDecodeRepo = new Mock<IVinDecodeRepository>();
            mockVinDecodeRepo.Setup(repo => repo.CheckVINExists(vin8, modelYearCode)).Returns(false);

            var controller = new VinDecodeController();
            var result = await controller.VinDecode(vin8, thirdPartyFlag);
        }



    }
}
