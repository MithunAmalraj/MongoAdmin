using MongoAdmin.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace MongoAdmin.DAL
{
    public class DatabaseDAL : BaseDAL
    {
        public DatabaseDAL(AppConfigSettings _appSettings) : base(_appSettings)
        {
        }

        public List<Database> SelectAllDatabase()
        {
            List<Database> databaseList = new List<Database>();
            Database databaseObj = new Database();
            try
            {
                using (var cursor = mongoClient.ListDatabases())
                {
                    while (cursor.MoveNext())
                    {
                        foreach (var doc in cursor.Current)
                        {
                            databaseObj = new Database();
                            databaseObj.DatabaseName = doc["name"].ToString();
                            databaseObj.SizeOnDisk = doc["sizeOnDisk"].ToString();
                            databaseList.Add(databaseObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return databaseList;
        }

        public string GetCurrentDatabase()
        {
            try
            {
                return DBName;
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return "";
        }

        public void CreateDatabase(string DatabaseName, string CollectionName, bool IsCappedCollection, long CappedCollectionSize)
        {
            try
            {
                var db = mongoClient.GetDatabase(DatabaseName);
                if (IsCappedCollection == true)
                {
                    var options = new CreateCollectionOptions { Capped = IsCappedCollection, MaxSize = CappedCollectionSize };
                    db.CreateCollection(CollectionName, options);
                }
                else
                {
                    db.CreateCollection(CollectionName);
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }

        public string DropDatabase(string DatabaseName)
        {
            try
            {
                mongoClient.DropDatabase(DatabaseName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return "";
        }

        public DatabaseStats GetDatabaseStats(string DatabaseName)
        {
            DatabaseStats myObj = new DatabaseStats();
            try
            {
                var DB = mongoClient.GetDatabase(DatabaseName);
                var command = new BsonDocument { { "dbstats", 1 } };
                var stats = DB.RunCommand<BsonDocument>(command);
                myObj = BsonSerializer.Deserialize<DatabaseStats>(stats);
                //IMongoDatabase db = mongoClient.GetDatabase(DatabaseName);
                //db.RunCommand("serverStatus");
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return myObj;
        }
    }
}