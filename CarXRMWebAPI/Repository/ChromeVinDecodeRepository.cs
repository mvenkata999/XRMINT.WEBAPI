using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CarXRMWebAPI.Models;
using Dapper;
using ChromeServiceReference;

namespace CarXRMWebAPI.Repository
{
    public class ChromeVinDecodeRepository : VinDecodeRepository
    {
        private string connectionStringChrome;
        private string CONNECTIONSTRINGMASK = "Data Source=tcp:{0};Connection Timeout=60;Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}";
        public ChromeVinDecodeRepository()
        {
            connectionStringChrome = @"Server=HouSql02Prd;Database=Chrome;Persist Security Info=True;User ID=sa;password=whitehouse";
        }
        public override bool CheckVINExists(string vin8, string modelYearCode)
        {
            bool bRetCd = false;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    bRetCd = dbConnection.ExecuteScalar<bool>("select count(1) from dbo.tblChromeVin8 where Vin8=@vin8 AND ModelYearCode=@yearCode", new { vin8, modelYearCode });
                }

                return bRetCd;
            }
            catch (Exception ex)
            {
                return bRetCd;
            }
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionStringChrome);
            }
        }

        public override IEnumerable<DecodedVehicle> GetVinData(string vin)
        {
            try
            {
                DataTable v8 = new DataTable();
                using (IDbConnection dbConnection = Connection)
                {   
                    dbConnection.Open();
                    return dbConnection.Query<DecodedVehicle>("SELECT * FROM dbo.UDF_CRM_VINDECODE_CHROME_GENERIC_TOPROW('" + vin + "')");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async override Task<IResponseObject> ThirdPartyVinDecode(string vin)
        {
            IResponseObject respObj = new ResponseObject();

            try
            {   
                DataTable dt = new DataTable();
                //dt = servFramework.getAccessCredentials();
                //var dataRow = (from product in dt.AsEnumerable()
                //               where product.Field<int>("VendorID") == 1004 && product.Field<int>("ProductCode") == 1001003
                //               select product).First();
                //var dataRow = "";

                //if (dataRow != null)
                //{

                AccountInfo chromeAccountInfo = new AccountInfo();
                chromeAccountInfo.number = "301720";
                chromeAccountInfo.secret = "fb2ccea424694b17";
                chromeAccountInfo.country = "US";
                chromeAccountInfo.language = "en";

                Description7bPortTypeClient service = new Description7bPortTypeClient();

                BaseRequest baseRequest = new BaseRequest();
                baseRequest.accountInfo = chromeAccountInfo;

                //VersionInfo versionInfo = service.getVersionInfo(baseRequest);
                //VersionInfoData[] data = versionInfo.data;

                VehicleDescription vehicleInfo = null;
                VehicleDescriptionRequest vehRequest = new VehicleDescriptionRequest();
                vehRequest.accountInfo = chromeAccountInfo;

                vehRequest.Items = new object[1];
                vehRequest.Items[0] = vin;
                vehRequest.ItemsElementName = new ItemsChoiceType[1];
                vehRequest.ItemsElementName[0] = ItemsChoiceType.vin;

                describeVehicleResponse descVehResp = new describeVehicleResponse();

                descVehResp = await service.describeVehicleAsync(vehRequest);
                //await Task.WhenAny(tskVehicleInfo);
                
                respObj.ErrId = 0;                
                respObj.ObjectList.Add(descVehResp.VehicleDescription);

                return respObj;
            }
            catch (Exception ex)
            {
                respObj.ErrId = 1007;
                respObj.ErrDesc = ex.Message;
                return respObj;
            }
        }

        private bool UpdateCarrModelCode(string ServerName)
        {
            string DataBase = "Chrome";
            SqlConnection _Conn = new SqlConnection("Data Source=" + ServerName + ";Connection Timeout=60;Initial Catalog=" + DataBase + ";Persist Security Info=True;User ID=ChromeLookup;Password=ChromeL1!");
            bool bRetCd = false;
            try
            {
                SqlCommand _SqlCmd = new SqlCommand();
                _SqlCmd.CommandText = "P_Update_Carr_ModelCode";
                _SqlCmd.CommandType = CommandType.StoredProcedure;

                _SqlCmd.Connection = _Conn;
                _Conn.Open();
                _SqlCmd.ExecuteNonQuery();
                _Conn.Close();

                bRetCd = true;
            }
            catch (Exception ex)
            {
                bRetCd = false;
            }
            finally
            {
                if (_Conn.State == ConnectionState.Open)
                {
                    _Conn.Close();
                }
            }

            return bRetCd;
        }

        public override IResponseObject SaveVinDecode(IRequestObject reqObj)
        {
            IResponseObject respObj = new ResponseObject();

            try
            {   
                bool wasSuccessful = true;
                string vehicleDesc = "";

                if (reqObj != null)
                {
                    VehicleDescription vehicleInfo = (VehicleDescription)reqObj.ObjectList[0];
                    if (vehicleInfo != null)
                    {
                        Guid passthruGuid = Guid.NewGuid();
                        string passthru_id = Convert.ToString(passthruGuid) + "_WS";

                        VehicleDescriptionVinDescription vinDesc = vehicleInfo.vinDescription;

                        ResponseStatus vehicleDecodedStatus = vehicleInfo.responseStatus;

                        Engine[] vehicleDecodedEngine = vehicleInfo.engine;
                        Style[] vehicleDecodedStyle = vehicleInfo.style;
                        Standard[] vehicleDecodedStandard = vehicleInfo.standard;
                        GenericEquipment[] vehicleDecodedGenericEquipment = vehicleInfo.genericEquipment;
                        ConsumerInformation[] vehicleDecodedConsumerInformation = vehicleInfo.consumerInformation;
                        TechnicalSpecification[] vehicleDecodedTechnicalSpecification = vehicleInfo.technicalSpecification;
                        Color[] vehicleDecodedExteriorColor = vehicleInfo.exteriorColor;
                        Color[] vehicleDecodedInteriorColor = vehicleInfo.interiorColor;
                        GenericColor[] vehicleDecodedGerericColor = vehicleInfo.genericColor;

                        Option[] vehicleDecodedOptions = vehicleInfo.factoryOption;
                        Option[] vehicleDecodedOptionsAmbiguousOptions = vehicleInfo.factoryOption;

                        PriceRange vehicleDecodedBasePrice = vehicleInfo.basePrice;

                        string _ConnectionString = String.Format(CONNECTIONSTRINGMASK, "HouSql02Prd", "Chrome", "ChromeLookup", "ChromeL1!");
                        using (SqlConnection cnn = new SqlConnection(_ConnectionString))
                        {

                            SqlCommand sqlCmd = new SqlCommand();
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.CommandText = "P_Write_VehicleDescription";
                            sqlCmd.Connection = cnn;
                            SqlTransaction transaction;
                            cnn.Open();
                            transaction = cnn.BeginTransaction("VehicleDecoded");

                            try
                            {
                                string styleName = "";

                                //START table "style"

                                SqlCommand sqlCmdStyle = new SqlCommand();
                                sqlCmdStyle.CommandType = CommandType.StoredProcedure;
                                sqlCmdStyle.CommandText = "P_Write_VehicleStyle";
                                sqlCmdStyle.Connection = cnn;

                                SqlCommand sqlCmdBodyStyle = new SqlCommand();
                                sqlCmdBodyStyle.CommandType = CommandType.StoredProcedure;
                                sqlCmdBodyStyle.CommandText = "P_Write_VehicleBodyStyle";
                                sqlCmdBodyStyle.Connection = cnn;

                                if (vehicleDecodedStyle != null && vehicleDecodedStyle.Length > 0)
                                {
                                    List<Style> styleList = new List<Style>();
                                    styleList = vehicleDecodedStyle.ToList();
                                    int StyleSeqNumber = 0;
                                    foreach (Style vehStyle in styleList)
                                    {
                                        sqlCmdStyle.Parameters.Clear();
                                        sqlCmdStyle.Parameters.AddWithValue("@InputRowNumber", StyleSeqNumber.ToString());
                                        sqlCmdStyle.Parameters.AddWithValue("@PassThruId", passthru_id);
                                        sqlCmdStyle.Parameters.AddWithValue("@style_id", vehStyle.id.ToString());
                                        sqlCmdStyle.Parameters.AddWithValue("@division_id", vehStyle.division.id.ToString());
                                        sqlCmdStyle.Parameters.AddWithValue("@sub_division_id", vehStyle.subdivision.id.ToString());
                                        sqlCmdStyle.Parameters.AddWithValue("@model_id", vehStyle.model.id.ToString());
                                        sqlCmdStyle.Parameters.AddWithValue("@base_msrp", vehStyle.basePrice == null ? "" : Convert.ToString(vehStyle.basePrice.msrp));
                                        sqlCmdStyle.Parameters.AddWithValue("@base_invoice", vehStyle.basePrice == null ? "" : Convert.ToString(vehStyle.basePrice.invoice));
                                        sqlCmdStyle.Parameters.AddWithValue("@dest_chrg", vehStyle.basePrice == null ? "" : Convert.ToString(vehStyle.basePrice.destination));
                                        sqlCmdStyle.Parameters.AddWithValue("@is_price_unknown", vehStyle.basePrice == null ? "true" : Convert.ToString(vehStyle.basePrice.unknown));
                                        sqlCmdStyle.Parameters.AddWithValue("@market_class_id", vehStyle.marketClass == null ? "" : Convert.ToString(vehStyle.marketClass.id));
                                        sqlCmdStyle.Parameters.AddWithValue("@model_year", vehStyle.modelYear == null ? "" : Convert.ToString(vehStyle.modelYear));
                                        sqlCmdStyle.Parameters.AddWithValue("@style_name", vehStyle.name == null ? "" : Convert.ToString(vehStyle.name));
                                        sqlCmdStyle.Parameters.AddWithValue("@style_name_wo_trim", vehStyle.nameWoTrim == null ? "" : Convert.ToString(vehStyle.nameWoTrim));
                                        sqlCmdStyle.Parameters.AddWithValue("@trim_name", vehStyle.trim == null ? "" : Convert.ToString(vehStyle.trim));
                                        sqlCmdStyle.Parameters.AddWithValue("@mfr_model_code", vehStyle.mfrModelCode == null ? "" : Convert.ToString(vehStyle.mfrModelCode));
                                        sqlCmdStyle.Parameters.AddWithValue("@is_fleet_only", Convert.ToString(vehStyle.fleetOnly));
                                        sqlCmdStyle.Parameters.AddWithValue("@is_model_fleet", Convert.ToString(vehStyle.modelFleet));
                                        sqlCmdStyle.Parameters.AddWithValue("@pass_doors", Convert.ToString(vehStyle.passDoors));
                                        sqlCmdStyle.Parameters.AddWithValue("@alt_model_name", vehStyle.altModelName == null ? "" : Convert.ToString(vehStyle.altModelName));
                                        sqlCmdStyle.Parameters.AddWithValue("@alt_style_name", vehStyle.altStyleName == null ? "" : Convert.ToString(vehStyle.altStyleName));
                                        sqlCmdStyle.Parameters.AddWithValue("@alt_body_type", vehStyle.altBodyType == null ? "" : Convert.ToString(vehStyle.altBodyType));
                                        sqlCmdStyle.Parameters.AddWithValue("@drivetrain", vehStyle.drivetrain.ToString());

                                        if (vehStyle.bodyType.Length > 0)
                                        {
                                            //this goes into table body_type to be consistant with the FTP process
                                            List<StyleBodyType> bodyStyleList = new List<StyleBodyType>();
                                            bodyStyleList = vehStyle.bodyType.ToList();
                                            int ListSeqNumber = 0;
                                            foreach (StyleBodyType vehBodyStyle in bodyStyleList)
                                            {
                                                sqlCmdBodyStyle.Parameters.Clear();
                                                sqlCmdBodyStyle.Parameters.AddWithValue("@InputRowNumber", ListSeqNumber.ToString());
                                                sqlCmdBodyStyle.Parameters.AddWithValue("@PassThruId", passthru_id);
                                                sqlCmdBodyStyle.Parameters.AddWithValue("@style_id", vehStyle.id.ToString());
                                                sqlCmdBodyStyle.Parameters.AddWithValue("@body_type_name", vehBodyStyle.Value);
                                                sqlCmdBodyStyle.Parameters.AddWithValue("@body_type_id", Convert.ToString(vehBodyStyle.id));
                                                sqlCmdBodyStyle.Parameters.AddWithValue("@is_primary", Convert.ToString(vehBodyStyle.primary.ToString()));

                                                sqlCmdBodyStyle.Transaction = transaction;
                                                sqlCmdBodyStyle.ExecuteNonQuery();
                                                ListSeqNumber++;
                                            }

                                        }

                                        sqlCmdStyle.Transaction = transaction;
                                        sqlCmdStyle.ExecuteNonQuery();

                                        if (StyleSeqNumber == 0)
                                            styleName = vehStyle.name == null ? "" : Convert.ToString(vehStyle.name);

                                        StyleSeqNumber++;
                                    }
                                }

                                //START table "vehicle_description"
                                sqlCmd.Parameters.AddWithValue("@InputRowNumber", 0);
                                sqlCmd.Parameters.AddWithValue("@PassThruId", passthru_id);
                                sqlCmd.Parameters.AddWithValue("@Vin", vinDesc.vin);
                                sqlCmd.Parameters.AddWithValue("@GvwrLow", vinDesc.gvwr == null ? "" : Convert.ToString(vinDesc.gvwr.low));
                                sqlCmd.Parameters.AddWithValue("@GvwrHigh", vinDesc.gvwr == null ? "" : Convert.ToString(vinDesc.gvwr.high));

                                sqlCmd.Parameters.AddWithValue("@ManufacturerID", vinDesc.WorldManufacturerIdentifier == null ? "" : Convert.ToString(vinDesc.WorldManufacturerIdentifier));
                                sqlCmd.Parameters.AddWithValue("@ManufactureIdCode", vinDesc.ManufacturerIdentificationCode == null ? "" : vinDesc.ManufacturerIdentificationCode);

                                try
                                {
                                    if (vinDesc.restraintTypes != null && vinDesc.restraintTypes.Length > 0)
                                    {
                                        string RestraintTypes = "";

                                        foreach (CategoryDefinition rt in vinDesc.restraintTypes)
                                        {
                                            RestraintTypes += rt.category.id + ";";
                                        }
                                        RestraintTypes.TrimEnd(new char[] { ';' });
                                        sqlCmd.Parameters.AddWithValue("@RestraintType", RestraintTypes);
                                    }
                                    else
                                    {
                                        sqlCmd.Parameters.AddWithValue("@RestraintType", "");
                                    }

                                    if (vinDesc.marketClass != null && vinDesc.marketClass.Length > 0)
                                    {
                                        string MarketClasses = "";

                                        DataTable dtMktDef = new DataTable();
                                        dtMktDef.TableName = "MarketClasses_Definitions";
                                        dtMktDef.Columns.Add("id");
                                        dtMktDef.Columns.Add("MarketClassDefination");

                                        foreach (IdentifiedString mc in vinDesc.marketClass)
                                        {
                                            MarketClasses += Convert.ToString(mc.id) + ";";

                                            DataRow drMktDef = dtMktDef.NewRow();
                                            drMktDef["id"] = Convert.ToString(mc.id);
                                            drMktDef["MarketClassDefination"] = mc.Value;
                                            dtMktDef.Rows.Add(drMktDef);

                                            //TODO write table to SQL
                                        }
                                        MarketClasses.TrimEnd(new char[] { ';' });
                                        sqlCmd.Parameters.AddWithValue("@MarketClassId", MarketClasses);
                                    }
                                    else
                                    {
                                        sqlCmd.Parameters.AddWithValue("@MarketClassId", "");
                                    }

                                    sqlCmd.Parameters.AddWithValue("@ModelYear", Convert.ToString(vinDesc.modelYear));
                                    sqlCmd.Parameters.AddWithValue("@Division", vinDesc.division == null ? "" : Convert.ToString(vinDesc.division));
                                    sqlCmd.Parameters.AddWithValue("@ModelName", vinDesc.modelName == null ? "" : Convert.ToString(vinDesc.modelName));
                                    sqlCmd.Parameters.AddWithValue("@StyleName", vinDesc.styleName == null ? "" : Convert.ToString(vinDesc.styleName));
                                    sqlCmd.Parameters.AddWithValue("@BodyType", vinDesc.bodyType == null ? "" : Convert.ToString(vinDesc.bodyType));
                                    sqlCmd.Parameters.AddWithValue("@DrivingWheels", vinDesc.drivingWheels == null ? "" : Convert.ToString(vinDesc.drivingWheels));
                                    sqlCmd.Parameters.AddWithValue("@Built", vinDesc.built == null ? "" : Convert.ToString(vinDesc.built));
                                    sqlCmd.Parameters.AddWithValue("@Country", vehicleInfo.country == null ? "" : Convert.ToString(vehicleInfo.country));
                                    sqlCmd.Parameters.AddWithValue("@Language", vehicleInfo.language == null ? "" : Convert.ToString(vehicleInfo.language));
                                    sqlCmd.Parameters.AddWithValue("@BestMakeName", vehicleInfo.bestMakeName == null ? "" : Convert.ToString(vehicleInfo.bestMakeName));
                                    sqlCmd.Parameters.AddWithValue("@BestModelName", vehicleInfo.bestModelName == null ? "" : Convert.ToString(vehicleInfo.bestModelName));
                                    sqlCmd.Parameters.AddWithValue("@BestStyleName", vehicleInfo.bestStyleName == null ? "" : Convert.ToString(vehicleInfo.bestStyleName));
                                    sqlCmd.Parameters.AddWithValue("@StyleTblStyleName", styleName);
                                    sqlCmd.Parameters.AddWithValue("@BestTrimName", vehicleInfo.bestTrimName == null ? "" : Convert.ToString(vehicleInfo.bestTrimName));
                                    if (vehicleDecodedBasePrice != null)
                                    {
                                        sqlCmd.Parameters.AddWithValue("@BaseMsrpLow", vehicleDecodedBasePrice.msrp.low == null ? "" : Convert.ToString(vehicleDecodedBasePrice.msrp.low));
                                        sqlCmd.Parameters.AddWithValue("@BaseMsrpHigh", vehicleDecodedBasePrice.msrp.high == null ? "" : Convert.ToString(vehicleDecodedBasePrice.msrp.high));
                                        sqlCmd.Parameters.AddWithValue("@BaseInvLow", vehicleDecodedBasePrice.invoice.low == null ? "" : Convert.ToString(vehicleDecodedBasePrice.invoice.low));
                                        sqlCmd.Parameters.AddWithValue("@BaseInvHigh", vehicleDecodedBasePrice.invoice.high == null ? "" : Convert.ToString(vehicleDecodedBasePrice.invoice.high));
                                        sqlCmd.Parameters.AddWithValue("@DestChrgLow", vehicleDecodedBasePrice.destination.low == null ? "" : Convert.ToString(vehicleDecodedBasePrice.destination.low));
                                        sqlCmd.Parameters.AddWithValue("@DestChrgHigh", vehicleDecodedBasePrice.destination.high == null ? "" : Convert.ToString(vehicleDecodedBasePrice.destination.high));
                                        sqlCmd.Parameters.AddWithValue("@IsPriceUnknown", Convert.ToInt16(vehicleDecodedBasePrice.unknown)).ToString();
                                    }
                                    else
                                    {
                                        sqlCmd.Parameters.AddWithValue("@BaseMsrpLow", "");
                                        sqlCmd.Parameters.AddWithValue("@BaseMsrpHigh", "");
                                        sqlCmd.Parameters.AddWithValue("@BaseInvLow", "");
                                        sqlCmd.Parameters.AddWithValue("@BaseInvHigh", "");
                                        sqlCmd.Parameters.AddWithValue("@DestChrgLow", "");
                                        sqlCmd.Parameters.AddWithValue("@DestChrgHigh", "");
                                        sqlCmd.Parameters.AddWithValue("@IsPriceUnknown", "true");
                                    }

                                    sqlCmd.Parameters.AddWithValue("@EntrySource", "WebService");
                                }
                                catch (Exception ex)
                                {

                                }

                                sqlCmd.Transaction = transaction;
                                sqlCmd.ExecuteNonQuery();
                                transaction.Commit();
                            }
                            catch (Exception exProcessTable)
                            {
                                transaction.Rollback();
                                wasSuccessful = false;
                            }
                            finally
                            {
                                cnn.Close();
                            }

                            if (wasSuccessful == true)
                            {
                                if (!UpdateCarrModelCode("HouSql02Prd"))
                                {
                                    respObj.ErrId = 1007;
                                    respObj.ErrDesc = "Error occurred while updating carr model code";
                                }
                                else
                                {
                                    respObj.ErrId = 0;
                                    respObj.ErrDesc = "";
                                }
                            }
                        }
                    }
                    else
                    {   
                        respObj.ErrId = 1003;
                        respObj.ErrDesc = "Required vehicle description data set is missing.";
                    }
                }
                else
                {   
                    respObj.ErrId = 1001;
                    respObj.ErrDesc = "Invalid input data set to save.";
                }

                return respObj;
            }
            catch (Exception ex)
            {
                respObj = new ResponseObject();
                respObj.ErrId = 1002;
                respObj.ErrDesc = "Error occurred while saving vehicle description to database.";
                return respObj;
            }
        }
    }
}
