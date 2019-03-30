using MongoAdmin.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoAdmin.DAL
{
    public class AuthenticateDAL
    {
        public ApplicationUser GetUserForUserName(string UserName, string Password, string MongoURL)
        {
            ApplicationUser UserObj = new ApplicationUser();
            try
            {
                var settings = MongoClientSettings.FromUrl(MongoUrl.Create(MongoURL));
                var mongoClient = new MongoClient(settings);
                string DBName = MongoUrl.Create(MongoURL).DatabaseName;
                var db = mongoClient.GetDatabase(DBName);
                var collection = db.GetCollection<ApplicationUser>("ApplicationUser");
                UserObj = collection.AsQueryable()
                                .Where(p => p.UserName == UserName && p.Password == Password).SingleOrDefault();
            }
            catch (Exception ex)
            {
            }
            return UserObj;
        }
        public MongoUser GetMongoUserForId(int MongoUserId, string MongoURL)
        {
            MongoUser MongoUserObj = new MongoUser();
            try
            {
                var settings = MongoClientSettings.FromUrl(MongoUrl.Create(MongoURL));
                var mongoClient = new MongoClient(settings);
                string DBName = MongoUrl.Create(MongoURL).DatabaseName;
                var db = mongoClient.GetDatabase(DBName);
                var collection = db.GetCollection<MongoUser>("MongoUser");
                MongoUserObj = collection.AsQueryable()
                                .Where(p => p._id == MongoUserId).SingleOrDefault();
            }
            catch (Exception ex)
            {

            }
            return MongoUserObj;
        }

        public List<Database> GetAllDatabases(string MongoURL)
        {
            List<Database> databaseList = new List<Database>();
            Database databaseObj = new Database();
            try
            {
                var settings = MongoClientSettings.FromUrl(MongoUrl.Create(MongoURL));
                var mongoClient = new MongoClient(settings);
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
            catch(Exception ex)
            {

            }
            return databaseList;
        }

        public DatabaseStats GetDatabaseStats(string MongoURL, string DatabaseName)
        {
            DatabaseStats myObj = new DatabaseStats();
            try
            {
                var settings = MongoClientSettings.FromUrl(MongoUrl.Create(MongoURL));
                var mongoClient = new MongoClient(settings);
                var DB = mongoClient.GetDatabase(DatabaseName);
                var command = new BsonDocument { { "dbstats", 1 } };
                var stats = DB.RunCommand<BsonDocument>(command);
                myObj = BsonSerializer.Deserialize<DatabaseStats>(stats);
            }
            catch (Exception ex)
            {
            }
            return myObj;
        }
    }
}
