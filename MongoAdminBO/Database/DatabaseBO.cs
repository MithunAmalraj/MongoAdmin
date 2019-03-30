using MongoAdmin.DAL;
using MongoAdmin.Logger;
using MongoAdmin.Models;
using System;
using System.Collections.Generic;

namespace MongoAdmin.BO
{
    public class DatabaseBO : BaseBO
    {
        private DatabaseDAL _databaseDAO;

        public DatabaseBO(AppConfigSettings _appSettings) : base(_appSettings)
        {
            _databaseDAO = new DatabaseDAL(_appSettings);
        }

        public DatabaseStats GetDatabaseStats(string DatabaseName)
        {
            DatabaseStats myObj = new DatabaseStats();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName, _localAppSettings);
                myObj = _databaseDAO.GetDatabaseStats(DatabaseName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return myObj;
        }

        public List<Database> SelectAllDatabase()
        {
            List<Database> databaseList = new List<Database>();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, "", _localAppSettings);
                databaseList = _databaseDAO.SelectAllDatabase();
                databaseList.Sort((x, y) => x.DatabaseName.CompareTo(y.DatabaseName));
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return databaseList;
        }

        public string GetCurrentDatabase()
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, "", _localAppSettings);
                return _databaseDAO.GetCurrentDatabase();
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return "";
        }

        public void CreateDatabase(string DatabaseName, string CollectionName, bool IsCappedCollection, long CappedCollectionSize)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName + "|" + IsCappedCollection + "|" + CappedCollectionSize, _localAppSettings);
                _databaseDAO.CreateDatabase(DatabaseName, CollectionName, IsCappedCollection, CappedCollectionSize);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }

        public string DropDatabase(string DatabaseName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName, _localAppSettings);
                return _databaseDAO.DropDatabase(DatabaseName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return "";
        }
    }
}