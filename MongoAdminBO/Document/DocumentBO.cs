using MongoAdmin.DAL;
using MongoAdmin.Logger;
using MongoAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MongoAdmin.BO
{
    public class DocumentBO : BaseBO
    {
        private DocumentDAL _documentDAO;

        public DocumentBO(AppConfigSettings _appSettings) : base(_appSettings)
        {
            _documentDAO = new DocumentDAL(_appSettings);
        }

        public bool InsertDocument(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, cmd, DatabaseName + "|" + CollectionName, _localAppSettings);
                return _documentDAO.InsertDocument(cmd, DatabaseName, CollectionName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return false;
        }

        //Update existing documents based on condition
        public bool UpdateDocument(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, cmd, DatabaseName + "|" + CollectionName, _localAppSettings);
                return _documentDAO.UpdateDocument(cmd, DatabaseName, CollectionName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return false;
        }

        //Replace one existing document
        public bool ReplaceOneDocument(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, cmd, DatabaseName + "|" + CollectionName, _localAppSettings);
                return _documentDAO.ReplaceOneDocument(cmd, DatabaseName, CollectionName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return false;
        }

        //Update if document exists or insert document
        public bool UpsertDocument(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, cmd, DatabaseName + "|" + CollectionName, _localAppSettings);
                return _documentDAO.UpsertDocument(cmd, DatabaseName, CollectionName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return false;
        }

        //Delete existing documents based on condition
        public bool DeleteDocument(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, cmd, DatabaseName + "|" + CollectionName, _localAppSettings);
                return _documentDAO.DeleteDocument(cmd, DatabaseName, CollectionName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return false;
        }

        //Get documents from collection
        public DataSet GetDocuments(MongoCommand cmd, string DatabaseName, string CollectionName)
        {
            DataSet ds = new DataSet();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, cmd, DatabaseName + "|" + CollectionName, _localAppSettings);
                return _documentDAO.GetDocuments(cmd, DatabaseName, CollectionName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return ds;
        }

        //Get documents from collection
        public DataSet GetDocumentsWithPagination(MongoCommand cmd, string DatabaseName, string CollectionName, int PageCount, int PageNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, cmd, DatabaseName + "|" + CollectionName + "|" + PageCount + "|" + PageNumber, _localAppSettings);
                return _documentDAO.GetDocumentsWithPagination(cmd, DatabaseName, CollectionName, PageCount, PageNumber);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return ds;
        }

        public long GetTotalDocumentsInCollectionCount(string DatabaseName, string CollectionName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName, _localAppSettings);
                var count = _documentDAO.GetTotalDocumentsInCollectionCount(DatabaseName, CollectionName);
                return count;
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return 0;
        }

        public List<DocumentFields> GetFieldNames(string DatabaseName, string CollectionName)
        {
            List<DocumentFields> documentFieldsList = new List<DocumentFields>();
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName, _localAppSettings);
                documentFieldsList = _documentDAO.GetFieldNames(DatabaseName, CollectionName);
                documentFieldsList.Sort((x, y) => x.FieldName.CompareTo(y.FieldName));
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return documentFieldsList;
        }

        public async Task<DataTable> FindDocumentByFieldValueAsync(string DatabaseName, string CollectionName)
        {
            try
            {
                _logObj.LogData("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, DatabaseName + "|" + CollectionName, _localAppSettings);
                DataTable dt = await _documentDAO.FindDocumentByFieldValueAsync(DatabaseName, CollectionName);
                return dt;
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.BO", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return null;
        }
    }
}