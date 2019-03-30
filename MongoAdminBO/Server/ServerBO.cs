using MongoAdmin.DAL;
using MongoAdmin.Logger;
using MongoAdmin.Models;
using System;

namespace MongoAdmin.BO
{
    public class ServerBO : BaseBO
    {
        private ServerDAL _serverDAO;

        public ServerBO(AppConfigSettings _appSettings) : base(_appSettings)
        {
            _serverDAO = new ServerDAL(_appSettings);
        }

        public ServerStats GetServerStats()
        {
            ServerStats myObj = new ServerStats();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, "", _localAppSettings);
                myObj = _serverDAO.GetServerStats();
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return myObj;
        }
    }
}