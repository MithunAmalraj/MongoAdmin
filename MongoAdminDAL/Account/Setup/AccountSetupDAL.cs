using MongoAdmin.Logger;
using MongoAdmin.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MongoAdmin.DAL
{
    public class AccountSetupDAL :BaseDAL
    {
        private Logging _logObj = new Logging();
        string MongoPath = "";
        string ExportPath = "";

        public AccountSetupDAL(AppConfigSettings _appSettings) : base(_appSettings)
        {

        }
    }
}