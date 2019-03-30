using MongoAdmin.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;

namespace MongoAdmin.DAL
{
    public class ServerDAL : BaseDAL
    {
        public ServerDAL(AppConfigSettings _appSettings) : base(_appSettings)
        {
        }

        public ServerStats GetServerStats()
        {
            ServerStats myObj = new ServerStats();
            try
            {
                var DB = mongoClient.GetDatabase("admin");
                var command = new BsonDocument { { "serverStatus", 1 } };
                var stats = DB.RunCommand<BsonDocument>(command);
                myObj = BsonSerializer.Deserialize<ServerStats>(stats);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return myObj;
        }
    }
}