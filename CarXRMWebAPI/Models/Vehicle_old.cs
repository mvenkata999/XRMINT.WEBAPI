using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Model
{
    public class Vehicle 
    {
        private int _vehId;
        private int _vehYear;        
        private string _vehStockNo = string.Empty;
        private string _vehVIN = string.Empty;
        private string _vehMake = string.Empty;
        private string _vehModel = string.Empty;
     
        public int VehYear { get => _vehYear; set => _vehYear = value; }
        public string VehStockNo { get => _vehStockNo; set => _vehStockNo = value; }
        public string VehMake { get => _vehMake; set => _vehMake = value; }
        public string VehModel { get => _vehModel; set => _vehModel = value; }
        public string VehVIN { get => _vehVIN; set => _vehVIN = value; }
        public int VehId { get => _vehId; set => _vehId = value; }

        DetailID
        StockNbr
        DealerID
        VehVIN
        Year
        Make
        MakeID
        Model
        ModelID
        ModelCode
        SalesCode
        Style
        Body
        Class
        Trim
        Transmission
        Engine
        EngineSize
        Doors
        DriveTrain
        IntColor
        ExtColor
        IntColorCode
        ExtColorCode
        VehStatus
        VehMileage
        WebPrice
        acv
        AskingPrice
        CurrentCost
        BookInPrice
        MSRP
        WholesalePrice
        VehRetailPrice
        Certified
        CertType
        Used
        BookinDate
        Features
        Comments
        DetailSource
        Warranty
        WarrantyType
        WarrantyServiceContract
        PercentLabor
        PercentParts
        WarrantyDuration
        WarrantySystemsCovered
        CityMPG
        HwyMPG
        EstFuelCost
        Special
        SpecialPrice
        SpecialStart
        SpecialEnds
        PhotosUpdated
        ListingUpdated
        DateOnLot
        ListingTitle
        ListingDetails
        ListingDetailsHTML
        DetailsFrom
        FeaturesFrom
        ListingInfoFrom
        PricingFrom
        WarrantyFrom
        ImagesFrom
        RetailPriceCopy
        WebPriceCopy
        WebLink
        WebTemplateID
        CostFrom
        CertNum
        BuyersGuidePrinted
        WindowStickerPrinted
        UsedPricingFrom
        LotLocation
        ChromeStyleIDs
        isBookedOut
    
}
