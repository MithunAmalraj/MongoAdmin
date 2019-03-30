using MongoAdmin.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MongoAdmin.DAL
{
    public class DocumentDAL : BaseDAL
    {
        public MongoProvider mongoObj = new MongoProvider();
        private List<IDictionary<string, object>> parent = new List<IDictionary<string, object>>();

        public DocumentDAL(AppConfigSettings _appSettings) : base(_appSettings)
        {
        }

        //Insert new document
        public bool InsertDocument(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            try
            {
                var DB = mongoClient.GetDatabase(DatabaseName);
                var collection = DB.GetCollection<BsonDocument>(CollectionName);
                var documnt = new BsonDocument();
                if (cmd != null && cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    foreach (var item in cmd.Parameters)
                    {
                        var documntObj = new BsonDocument { { item.name, item.value } };
                        documnt.AddRange(documntObj);
                    }
                    collection.InsertOne(documnt);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return false;
        }

        //Update existing documents based on condition
        public bool UpdateDocument(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            try
            {
                bool IsAnyFilterConditionGiven = false;
                var DB = mongoClient.GetDatabase(DatabaseName);
                var collection = DB.GetCollection<BsonDocument>(CollectionName);
                var filter = new BsonDocument();
                var updateValues = new BsonDocument();
                if (cmd != null && cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    foreach (var item in cmd.Parameters)
                    {
                        if (item.name != null && item.value != null && item.isFilterCondition == true)
                        {
                            IsAnyFilterConditionGiven = true;
                            if (item.matchExact == true)
                            {
                                var multiplefilterObj = new BsonDocument(item.name, item.value);
                                filter.AddRange(multiplefilterObj);
                            }
                            else if (item.matchExact == false)
                            {
                                var multiplefilterObj = new BsonDocument { { item.name, new BsonDocument { { "$regex", item.value }, { "$options", "i" } } } };
                                filter.AddRange(multiplefilterObj);
                            }
                        }
                        else if (item.name != null && item.value != null && item.isFilterCondition == false)
                        {
                            var multipleUpdateObj = new BsonDocument(item.name, item.value);
                            updateValues.AddRange(multipleUpdateObj);
                        }
                    }
                    if (IsAnyFilterConditionGiven == true)
                    {
                        var update = new BsonDocument("$set", updateValues);
                        var result = collection.UpdateMany(filter, update);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return false;
        }

        //Replace one existing document
        public bool ReplaceOneDocument(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            try
            {
                bool IsAnyFilterConditionGiven = false;
                var DB = mongoClient.GetDatabase(DatabaseName);
                var collection = DB.GetCollection<BsonDocument>(CollectionName);
                var filter = new BsonDocument();
                var replacement = new BsonDocument();
                if (cmd != null && cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    foreach (var item in cmd.Parameters)
                    {
                        if (item.name != null && item.value != null && item.isFilterCondition == true)
                        {
                            IsAnyFilterConditionGiven = true;
                            if (item.matchExact == true)
                            {
                                var multiplefilterObj = new BsonDocument(item.name, item.value);
                                filter.AddRange(multiplefilterObj);
                            }
                            else if (item.matchExact == false)
                            {
                                var multiplefilterObj = new BsonDocument { { item.name, new BsonDocument { { "$regex", item.value }, { "$options", "i" } } } };
                                filter.AddRange(multiplefilterObj);
                            }
                        }
                        else if (item.name != null && item.value != null && item.isFilterCondition == false)
                        {
                            var replaceObj = new BsonDocument(item.name, item.value);
                            replacement.AddRange(replaceObj);
                        }
                    }
                    if (IsAnyFilterConditionGiven == true)
                    {
                        var result = collection.ReplaceOne(filter, replacement);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return false;
        }

        //Update if document exists or insert document
        public bool UpsertDocument(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            try
            {
                bool IsAnyFilterConditionGiven = false;
                var DB = mongoClient.GetDatabase(DatabaseName);
                var collection = DB.GetCollection<BsonDocument>(CollectionName);
                var filter = new BsonDocument();
                var updateValues = new BsonDocument();
                if (cmd != null && cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    foreach (var item in cmd.Parameters)
                    {
                        if (item.name != null && item.value != null && item.isFilterCondition == true)
                        {
                            IsAnyFilterConditionGiven = true;
                            if (item.matchExact == true)
                            {
                                var multiplefilterObj = new BsonDocument(item.name, item.value);
                                filter.AddRange(multiplefilterObj);
                            }
                            else if (item.matchExact == false)
                            {
                                var multiplefilterObj = new BsonDocument { { item.name, new BsonDocument { { "$regex", item.value }, { "$options", "i" } } } };
                                filter.AddRange(multiplefilterObj);
                            }
                        }
                        else if (item.name != null && item.value != null && item.isFilterCondition == false)
                        {
                            var multipleUpdateObj = new BsonDocument(item.name, item.value);
                            updateValues.AddRange(multipleUpdateObj);
                        }
                    }
                    if (IsAnyFilterConditionGiven == true)
                    {
                        var update = new BsonDocument("$set", updateValues);
                        var options = new UpdateOptions { IsUpsert = true };
                        var result = collection.UpdateMany(filter, update, options);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return false;
        }

        //Delete existing documents based on condition
        public bool DeleteDocument(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            try
            {
                bool IsAnyFilterConditionGiven = false;
                var DB = mongoClient.GetDatabase(DatabaseName);
                var collection = DB.GetCollection<BsonDocument>(CollectionName);
                var filter = new BsonDocument();
                if (cmd != null && cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    foreach (var item in cmd.Parameters)
                    {
                        if (item.name != null && item.value != null && item.isFilterCondition == true)
                        {
                            IsAnyFilterConditionGiven = true;
                            if (item.matchExact == true)
                            {
                                var multiplefilterObj = new BsonDocument(item.name, item.value);
                                filter.AddRange(multiplefilterObj);
                            }
                            else if (item.matchExact == false)
                            {
                                var multiplefilterObj = new BsonDocument { { item.name, new BsonDocument { { "$regex", item.value }, { "$options", "i" } } } };
                                filter.AddRange(multiplefilterObj);
                            }
                        }
                    }
                    if (IsAnyFilterConditionGiven == true)
                    {
                        var result = collection.DeleteMany(filter);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return false;
        }

        //Get documents from collection
        public DataSet GetDocuments(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            DataSet ds = new DataSet();
            try
            {
                var DB = mongoClient.GetDatabase(DatabaseName);
                var collection = DB.GetCollection<BsonDocument>(CollectionName);
                var filter = new BsonDocument();
                var sortFilter = "{";
                if (cmd != null && cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    foreach (var item in cmd.Parameters)
                    {
                        if (item.value != null && item.value != "")
                        {
                            if (item.matchExact == true)
                            {
                                var multiplefilterObj = new BsonDocument(item.name, item.value);
                                filter.AddRange(multiplefilterObj);
                            }
                            else if (item.matchExact == false)
                            {
                                var multiplefilterObj = new BsonDocument { { item.name, new BsonDocument { { "$regex", item.value }, { "$options", "i" } } } };
                                filter.AddRange(multiplefilterObj);
                            }
                        }

                        if (item.isSorted == true)
                        {
                            string AscorDesc = "1";
                            if (item.isSortedAscorDesc == false)
                                AscorDesc = "-1";
                            if (sortFilter != "{")
                                sortFilter += "," + item.name + ":" + AscorDesc;
                            else
                                sortFilter += item.name + ":" + AscorDesc;
                        }
                    }
                }
                sortFilter += "}";
                using (var cursor = collection.Find(filter).Sort(sortFilter).ToCursor())
                {
                    while (cursor.MoveNext())
                    {
                        foreach (var doc in cursor.Current)
                        {
                            Dictionary<string, object> dict = doc.ToDictionary();
                            parent.Add(dict);
                        }
                    }
                }
                DataTable dt = mongoObj.ToDataTable(parent);
                ds.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return ds;
        }

        //Get documents from collection
        public DataSet GetDocumentsWithPagination(MongoCommand cmd, string DatabaseName, string CollectionName,int PageCount, int PageNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                var DB = mongoClient.GetDatabase(DatabaseName);
                var collection = DB.GetCollection<BsonDocument>(CollectionName);
                var filter = new BsonDocument();
                var sortFilter = "{";
                if (cmd != null && cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    foreach (var item in cmd.Parameters)
                    {
                        if (item.value != null && item.value != "")
                        {
                            if (item.matchExact == true)
                            {
                                var multiplefilterObj = new BsonDocument(item.name, item.value);
                                filter.AddRange(multiplefilterObj);
                            }
                            else if (item.matchExact == false)
                            {
                                var multiplefilterObj = new BsonDocument { { item.name, new BsonDocument { { "$regex", item.value }, { "$options", "i" } } } };
                                filter.AddRange(multiplefilterObj);
                            }
                        }

                        if (item.isSorted == true)
                        {
                            string AscorDesc = "1";
                            if (item.isSortedAscorDesc == false)
                                AscorDesc = "-1";
                            if (sortFilter != "{")
                                sortFilter += "," + item.name + ":" + AscorDesc;
                            else
                                sortFilter += item.name + ":" + AscorDesc;
                        }
                    }
                }
                sortFilter += "}";
                using (var cursor = collection.Find(filter).Sort(sortFilter).Skip((PageNumber - 1) * PageCount).Limit(PageCount).ToCursor())
                {
                    while (cursor.MoveNext())
                    {
                        foreach (var doc in cursor.Current)
                        {
                            Dictionary<string, object> dict = doc.ToDictionary();
                            parent.Add(dict);
                        }
                    }
                }
                DataTable dt = mongoObj.ToDataTable(parent);
                ds.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return ds;
        }

        //Get documents from collection using pagination
        public long GetTotalDocumentsInCollectionCount(string DatabaseName, string CollectionName)
        {
            try
            {
                var DB = mongoClient.GetDatabase(DatabaseName);
                var collection = DB.GetCollection<BsonDocument>(CollectionName);
                var count = collection.CountDocuments(new BsonDocument());
                return count;
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return 0;
        }

        public List<DocumentFields> GetFieldNames(string DatabaseName, string CollectionName)
        {
            List<DocumentFields> documentFieldsList = new List<DocumentFields>();
            try
            {
                var DB = mongoClient.GetDatabase(DatabaseName);
                var collection = DB.GetCollection<BsonDocument>(CollectionName);
                //var a = collection.Find(x => true).Limit(1).FirstOrDefault();
                var filter = new BsonDocument();
                var a = collection.Find(filter).FirstOrDefault();
                //string json = a.ToJson();
                foreach (var elm in a.Elements)
                {
                    DocumentFields docObj = new DocumentFields();
                    docObj.FieldName = elm.Name;
                    documentFieldsList.Add(docObj);
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return documentFieldsList;
        }

        public async Task<DataTable> FindDocumentByFieldValueAsync(string DatabaseName, string CollectionName)
        {
            DataTable dt = new DataTable();
            try
            {
                IMongoDatabase db = mongoClient.GetDatabase(DatabaseName);
                var collection = db.GetCollection<BsonDocument>(CollectionName);
                FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;
                //filter = new BsonDocument(Field, Value);

                //FindOptions<BsonDocument> options = new FindOptions<BsonDocument> { };
                FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
                {
                    BatchSize = 100,
                    NoCursorTimeout = false
                };
                List<IDictionary<string, object>> parent = new List<IDictionary<string, object>>();
                using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter, options))
                {
                    var batch = 0;
                    while (await cursor.MoveNextAsync())
                    {
                        IEnumerable<BsonDocument> documents = cursor.Current;
                        batch++;
                        foreach (BsonDocument document in documents)
                        {
                            Dictionary<string, object> dict = document.ToDictionary();
                            parent.Add(dict);
                        }
                    }
                }
                dt = mongoObj.ToDataTable(parent);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return dt;
        }
    }
}