using MongoAdmin.DAL;
using MongoAdmin.Logger;
using MongoAdmin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoAdmin.BO
{
    public class CollectionBO : BaseBO
    {
        private CollectionDAL _collectionDAO;

        public CollectionBO(AppConfigSettings _appSettings) : base(_appSettings)
        {
            _collectionDAO = new CollectionDAL(_appSettings);
        }

        public void CreateCollection(string DatabaseName, string CollectionName, bool IsCappedCollection, long CappedCollectionSize)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName + "|" + IsCappedCollection + "|" + CappedCollectionSize, _localAppSettings);
                _collectionDAO.CreateCollection(DatabaseName, CollectionName, IsCappedCollection, CappedCollectionSize);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }

        public GenericResponse BackupandDropCollection(CollectionBackupAndRestore collResObj)
        {
            GenericResponse GR = new GenericResponse();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, collResObj, "", _localAppSettings);
                GR = _collectionDAO.BackupandDropCollection(collResObj);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, collResObj, ex.Message, _localAppSettings);
            }
            return GR;
        }

        public GenericResponse RestoreCollection(CollectionBackupAndRestore collResObj)
        {
            GenericResponse GR = new GenericResponse();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, collResObj, "", _localAppSettings);
                GR = _collectionDAO.RestoreCollection(collResObj);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, collResObj, ex.Message, _localAppSettings);
            }
            return GR;
        }

        public CollectionStats GetCollectionStats(string DatabaseName, string CollectionName)
        {
            CollectionStats myObj = new CollectionStats();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName, _localAppSettings);
                myObj = _collectionDAO.GetCollectionStats(DatabaseName, CollectionName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return myObj;
        }

        public void DropCollection(string DatabaseName, string CollectionName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName, _localAppSettings);
                _collectionDAO.DropCollection(DatabaseName, CollectionName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }

        public void DropIndex(string DatabaseName, string CollectionName, string IndexName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName + "|" + IndexName, _localAppSettings);
                _collectionDAO.DropIndex(DatabaseName, CollectionName, IndexName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }

        public List<Collection> SelectAllCollection(string DatabaseName)
        {
            List<Collection> collectionList = new List<Collection>();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName, _localAppSettings);
                collectionList = _collectionDAO.SelectAllCollection(DatabaseName);
                collectionList.Sort((x, y) => x.CollectionName.CompareTo(y.CollectionName));
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return collectionList;
        }

        public List<CollectionIndexList> GetAllCollectionIndexes(string DatabaseName, string CollectionName)
        {
            List<CollectionIndexList> indexList = new List<CollectionIndexList>();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName, _localAppSettings);
                indexList = _collectionDAO.GetAllCollectionIndexes(DatabaseName, CollectionName);
                indexList.Sort((x, y) => x.IndexName.CompareTo(y.IndexName));
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return indexList;
        }

        public void CreateIndex(string DatabaseName, string CollectionName, string FieldName, string IndexType)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName + "|" + FieldName + "|" + IndexType, _localAppSettings);
                _collectionDAO.CreateIndex(DatabaseName, CollectionName, FieldName, IndexType);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }

        public async Task CreateIndexAsync(string DatabaseName, string CollectionName, string FieldName, string IndexType)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName + "|" + FieldName + "|" + IndexType, _localAppSettings);
                await _collectionDAO.CreateIndexAsync(DatabaseName, CollectionName, FieldName, IndexType).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }

        public async Task DropIndexAsync(string DatabaseName, string CollectionName, string IndexName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName + "|" + IndexName, _localAppSettings);
                await _collectionDAO.DropIndexAsync(DatabaseName, CollectionName, IndexName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }
    }
}