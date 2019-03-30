using MongoAdmin.DAL;
using MongoAdmin.Logger;
using MongoAdmin.Models;
using System;
using System.Collections.Generic;

namespace MongoAdmin.BO
{
    public class AuthenticateBO
    {
        private Logging _logObj = new Logging();
        private AuthenticateDAL _authenticateDAO = new AuthenticateDAL();

        public UserLogin Login(string UserName, string Password, string MongoURL)
        {
            UserLogin UserLoginObj = new UserLogin();
            ApplicationUser AppUserObj = new ApplicationUser();
            try
            {
                AppUserObj = _authenticateDAO.GetUserForUserName(UserName, Password, MongoURL);
                if (AppUserObj != null)
                {
                    UserLoginObj.UserName = AppUserObj.UserName;
                    UserLoginObj.Password = AppUserObj.Password;
                    UserLoginObj.AvailableServers = new List<UserLoginMongoServer>();
                    foreach (var item in AppUserObj.MongoUser)
                    {
                        MongoUser MongoUserObj = new MongoUser();
                        MongoUserObj = _authenticateDAO.GetMongoUserForId(item.MongoUserId, MongoURL);
                        if (MongoUserObj != null && MongoUserObj._id != 0)
                        {
                            UserLoginMongoServer serverObj = new UserLoginMongoServer();
                            serverObj.MongoUserId = MongoUserObj._id;
                            serverObj.MongoServer = MongoUserObj.MongoServer;
                            serverObj.MongoPort = MongoUserObj.MongoPort;
                            serverObj.MongoDBName = MongoUserObj.MongoDBName;
                            serverObj.MongoPassword = MongoUserObj.MongoPassword;
                            serverObj.MongoUserName = MongoUserObj.MongoUserName;
                            serverObj.CreateAccess = item.CreateAccess;
                            serverObj.ReadAccess = item.ReadAccess;
                            serverObj.UpdateAccess = item.UpdateAccess;
                            serverObj.DeleteAccess = item.DeleteAccess;
                            serverObj.ExportAccess = item.ExportAccess;
                            serverObj.IsDefaultDB = item.IsDefaultDB;
                            UserLoginObj.AvailableServers.Add(serverObj);
                        }
                    }
                    return UserLoginObj;
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, null);
            }
            return null;
        }

        public List<Database> GetAllDatabases(string MongoURL)
        {
            List<Database> databaseList = new List<Database>();
            try
            {
                return _authenticateDAO.GetAllDatabases(MongoURL);
            }
            catch (Exception ex)
            {
            }
            return databaseList;
        }
        public DatabaseStats GetDatabaseStats(string MongoURL, string DatabaseName)
        {
            DatabaseStats myObj = new DatabaseStats();
            try
            {
                return _authenticateDAO.GetDatabaseStats(MongoURL, DatabaseName);
            }
            catch (Exception ex)
            {
            }
            return myObj;
        }
    }
}