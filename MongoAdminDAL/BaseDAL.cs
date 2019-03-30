using MongoAdmin.Logger;
using MongoAdmin.Models;
using MongoDB.Driver;
using System;

namespace MongoAdmin.DAL
{
    public class BaseDAL
    {
        public AppConfigSettings _localAppSettings = new AppConfigSettings();
        public MongoClient mongoClient = new MongoClient();
        public Logging _logObj = new Logging();
        public string DBName = "";
        public string URL = "";

        public BaseDAL(AppConfigSettings _appSettings)
        {
            try
            {
                _localAppSettings = _appSettings;
                if (_appSettings.MongoUserName != null && _appSettings.MongoUserName.Trim() != "")
                {
                    if (_appSettings.MongoDBName == "admin")
                    {
                        URL = "mongodb://" + _appSettings.MongoUserName + ":" + _appSettings.MongoPassword + "@" + _appSettings.MongoServer + ":" + _appSettings.MongoPort + "";
                    }
                    else if (_appSettings.MongoDBName != null && _appSettings.MongoDBName != "")
                    {
                        URL = "mongodb://" + _appSettings.MongoUserName + ":" + _appSettings.MongoPassword + "@" + _appSettings.MongoServer + ":" + _appSettings.MongoPort + "/" + _appSettings.MongoDBName;
                    }
                }
                else
                {
                    if (_appSettings.MongoDBName == "admin")
                    {
                        URL = "mongodb://" + _appSettings.MongoServer + ":" + _appSettings.MongoPort + "";
                    }
                    else if (_appSettings.MongoDBName != null && _appSettings.MongoDBName != "")
                    {
                        URL = "mongodb://" + _appSettings.MongoServer + ":" + _appSettings.MongoPort + "/" + _appSettings.MongoDBName;
                    }
                }

                DBName = MongoUrl.Create(URL).DatabaseName;

                var settings = MongoClientSettings.FromUrl(MongoUrl.Create(URL));

                mongoClient = new MongoClient(settings);
            }
            catch (Exception ex)
            {
            }
        }
    }
}