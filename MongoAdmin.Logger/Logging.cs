using System;
using Newtonsoft.Json;
using System.IO;

namespace MongoAdmin.Logger
{
    public class Logging
    {
        public void LogData(string Namespace, string ClassName, string MethodName, dynamic JSONObject, string value, dynamic LoginDetails)
        {
            try
            {
                string fileName = "Log" + DateTime.Now.Date.ToShortDateString() + ".txt";
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Log\" + fileName;
                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        fs.Close();
                    }
                }
                LogObject logObj = new LogObject();
                logObj.CreatedDate = DateTime.Now;
                logObj.Namespace = Namespace;
                logObj.ClassName = ClassName;
                logObj.MethodName = MethodName;
                logObj.JSONObject = JSONObject;
                logObj.value = value;
                logObj.LoginDetails = LoginDetails;
                File.AppendAllText(filePath, "\r\n" + JsonConvert.SerializeObject(logObj));
            }
            catch (Exception ex)
            {
            }
        }

        public void LogError(string Namespace, string ClassName, string MethodName, dynamic JSONObject, string value, dynamic LoginDetails)
        {
            try
            {
                string fileName = "ErrorLog" + DateTime.Now.Date.ToShortDateString() + ".txt";
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Log\" + fileName;
                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        fs.Close();
                    }
                }
                LogObject logObj = new LogObject();
                logObj.CreatedDate = DateTime.Now;
                logObj.Namespace = Namespace;
                logObj.ClassName = ClassName;
                logObj.MethodName = MethodName;
                logObj.JSONObject = JSONObject;
                logObj.value = value;
                logObj.LoginDetails = LoginDetails;
                File.AppendAllText(filePath, "\r\n" + JsonConvert.SerializeObject(logObj));
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class LogObject
    {
        public DateTime CreatedDate { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public dynamic JSONObject { get; set; }
        public string value { get; set; }
        public dynamic LoginDetails { get; set; }
    }
}