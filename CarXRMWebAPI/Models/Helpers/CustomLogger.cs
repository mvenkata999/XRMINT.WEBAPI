using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Helpers
{
    public sealed class CustomLogger : ICustomLogger
    {
        private static ICustomLogger _instance = null;
        private static readonly object mutex = new object();
        private IAppSettings _appSettings = new AppSettings();
        private string _strLogFile;

        public static ICustomLogger Instance
        {
            get
            {
                lock (mutex)
                {
                    if (_instance == null)
                    {
                        _instance = CreateInstance();
                    }
                    return _instance;
                }
            }
        }

        private static ICustomLogger CreateInstance()
        {
            return new CustomLogger();
        }

        public FileStream XRMFileStream
        {
            get
            {
                return new FileStream(StrLogFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
        }

        public StreamWriter XRMStreamWriter
        {
            get
            {
                return new StreamWriter(XRMFileStream);
            }
        }

        public string StrLogFile
        {
            get
            {
                return _strLogFile;
            }
        }
        public void LogIt(string strMsg)
        {
            try
            {
                //bool bLogToFileFlag = Convert.ToBoolean(_appSettings.GetAppSetting("EnableLogToFile"));
                bool bLogToFileFlag = true;
                if (bLogToFileFlag)
                {
                    //_strLogFile = _appSettings.GetAppSetting("LogFileDirectory") + "//" + "CarXRMPIPTesting.log";
                    //if (!Directory.Exists(_appSettings.GetAppSetting("LogFileDirectory")))
                    //{
                    //    Directory.CreateDirectory(_appSettings.GetAppSetting("LogFileDirectory"));
                    //}

                    _strLogFile = "C:\\TEMP\\UniversalAPI.Log";
                    if (!Directory.Exists("C:\\TEMP"))
                    {
                        Directory.CreateDirectory("C:\\TEMP");
                    }

                    FileStream fs = XRMFileStream;
                    StreamWriter mStreamWriter = XRMStreamWriter;
                    mStreamWriter.BaseStream.Seek(0, SeekOrigin.End);
                    string timeStamp = string.Format("{0:MM/dd/yyyy hh:mm:ss:ms tt}", DateTime.Now);
                    mStreamWriter.WriteLine(timeStamp + "\t" + strMsg);
                    mStreamWriter.Flush();
                    mStreamWriter.Close();
                    fs.Close();
                    fs.Dispose();
                }        
            }
            catch (Exception ex)
            {

            }
        }
    }
}
