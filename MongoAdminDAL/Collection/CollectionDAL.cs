using MongoAdmin.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace MongoAdmin.DAL
{
    public class CollectionDAL : BaseDAL
    {
        private string MongoPath = "";
        private string ExportPath = "";

        public CollectionDAL(AppConfigSettings _appSettings) : base(_appSettings)
        {
            if (_appSettings != null)
            {
                MongoPath = _appSettings.MongoServerPath;
                ExportPath = _appSettings.ExportPath;
            }
        }

        public CollectionStats GetCollectionStats(string DatabaseName, string CollectionName)
        {
            CollectionStats myObj = new CollectionStats();
            try
            {
                var db = mongoClient.GetDatabase(DatabaseName);
                var command = new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "collstats", CollectionName } });
                var stats = db.RunCommand(command);
                myObj = BsonSerializer.Deserialize<CollectionStats>(stats);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return myObj;
        }

        public void CreateCollection(string DatabaseName, string CollectionName, bool IsCappedCollection, long CappedCollectionSize)
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

        public void DropCollection(string DatabaseName, string CollectionName)
        {
            try
            {
                var db = mongoClient.GetDatabase(DatabaseName);
                db.DropCollection(CollectionName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }

        public GenericResponse BackupandDropCollection(CollectionBackupAndRestore collResObj)
        {
            GenericResponse GR = new GenericResponse();
            GR.response_code = 1;
            Process proc = null;
            try
            {
                var settings = MongoClientSettings.FromUrl(MongoUrl.Create(URL));
                MongoClient mongoClient = new MongoClient(settings);
                var DatabaseName = MongoUrl.Create(URL).DatabaseName;
                var db = mongoClient.GetDatabase(DatabaseName);

                string CollectionName = collResObj.CollectionName;
                string CollectionDate = "";

                if (CollectionName.Contains("APILogs"))
                {
                    CollectionDate = CollectionName.Replace("APILogs", "");
                }
                else if (CollectionName.Contains("BankLogs"))
                {
                    CollectionDate = CollectionName.Replace("BankLogs", "");
                }
                else if (CollectionName.Contains("DiagnosticLogs"))
                {
                    CollectionDate = CollectionName.Replace("DiagnosticLogs", "");
                }
                else if (CollectionName.Contains("ExceptionLogs"))
                {
                    CollectionDate = CollectionName.Replace("ExceptionLogs", "");
                }
                else if (CollectionName.Contains("LogError"))
                {
                    CollectionDate = CollectionName.Replace("LogError", "");
                }
                else if (CollectionName.Contains("LogNavigation"))
                {
                    CollectionDate = CollectionName.Replace("LogNavigation", "");
                }
                else if (CollectionName.Contains("PaymentLogs"))
                {
                    CollectionDate = CollectionName.Replace("PaymentLogs", "");
                }
                else
                {
                    GR.response_code = 1;
                    GR.response_message = "Invalid Collection";
                    return GR;
                }

                if (CollectionDate != "")
                {
                    DateTime CurrentDateTime = DateTime.Now;
                    DateTime CollectionDateTime = DateTime.ParseExact(CollectionDate, "yyyyMd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

                    if (CollectionDateTime.Date > CurrentDateTime.Date.AddDays(-Convert.ToInt32(7)))
                    {
                        GR.response_code = 1;
                        GR.response_message = "Collection Must Be Before 7 days";
                        return GR;
                    }

                    //DateTime myDate = DateTime.ParseExact("2018-12-20 00:01:01,531", "yyyy-MM-dd HH:mm:ss,fff",
                    //           System.Globalization.CultureInfo.InvariantCulture);
                    //if (CollectionDateTime.Date != myDate.Date)
                    //    continue;
                }

                string batFilePath = collResObj.BatchFilePath;
                string batFileName = "BackupAndRAR_" + CollectionName + ".bat";

                if (File.Exists(batFilePath + batFileName))
                {
                    GR.response_message = "Backup Already In Progress. Please wait.";
                    return GR;
                    //File.Delete(batFilePath + batFileName);
                }

                using (FileStream fs = File.Create(batFilePath + batFileName))
                {
                    fs.Close();
                }

                string BackupFileName = "BACKUP_" + DatabaseName + "_" + CollectionName;

                //Check If file already present in backup folder
                //if (File.Exists(MongoBackupRARPath + BackupFileName + ".zip"))
                //{
                //    if (DeleteBackupIfAlreadyPresent == "1")
                //    {
                //        File.Delete(MongoBackupRARPath + BackupFileName + ".zip");
                //        return;
                //    }
                //}

                using (StreamWriter sw = new StreamWriter(batFilePath + batFileName))
                {
                    sw.WriteLine("@echo off");
                    sw.WriteLine("set logfilename=backup_archive_log_%date%");
                    sw.WriteLine("set logfilename=%logfilename:/=-%");
                    sw.WriteLine("set logfilename=%logfilename:/=-%");
                    sw.WriteLine("set logfilename=%logfilename: =__%");
                    sw.WriteLine("set logfilename=%logfilename:.=_%");
                    sw.WriteLine("set logfilename=%logfilename::=-%");
                    sw.WriteLine("set currentdatefilename=backup_dump_log_%date%_%time%");
                    sw.WriteLine("set currentdatefilename=%currentdatefilename:/=-%");
                    sw.WriteLine("set currentdatefilename=%currentdatefilename:/=-%");
                    sw.WriteLine("set currentdatefilename=%currentdatefilename: =__%");
                    sw.WriteLine("set currentdatefilename=%currentdatefilename:.=_%");
                    sw.WriteLine("set currentdatefilename=%currentdatefilename::=-%");
                    sw.WriteLine("call :sub >> " + collResObj.LogFilePath + "%logfilename%.txt");
                    sw.WriteLine("exit /b");
                    sw.WriteLine(":sub");
                    sw.WriteLine("echo ********************************************************************************************************************************");
                    sw.WriteLine("echo COLLECTION NAME : " + CollectionName + "");
                    sw.WriteLine("REM Create a file name for the database output which contains the date and time.Replace any characters which might cause an issue.");
                    sw.WriteLine("set filename=" + BackupFileName + "");
                    sw.WriteLine("REM Check If File Is already backed up");
                    sw.WriteLine("If Exist \"" + collResObj.MongoBackupRARPath + "%filename%.zip\" (");
                    sw.WriteLine("    echo WINRAR backup file already exists \"" + collResObj.MongoBackupRARPath + "%filename%.zip\" -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("REM Check If Mongo dump exists in folder");
                    sw.WriteLine("If Not Exist \"" + collResObj.MongoServerPath + "mongodump.exe\" (");
                    sw.WriteLine("    echo MONGODUMP does not exist \"" + collResObj.MongoServerPath + "mongodump.exe\" -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("echo MONGODUMP backup started -  %date% - %time%");
                    sw.WriteLine("echo.> " + collResObj.LogFilePath + "%currentdatefilename%.txt");
                    sw.WriteLine("path=%path%;\"" + collResObj.MongoServerPath + "\"");
                    sw.WriteLine("mongodump --uri " + URL + " --collection " + CollectionName + " --out " + collResObj.MongoBackupPath + "%filename%\\ 2> " + collResObj.LogFilePath + "%currentdatefilename%.txt");
                    sw.WriteLine("IF %ERRORLEVEL% NEQ 0 (");
                    sw.WriteLine("    echo MONGODUMP backup failed. mongodump --uri " + URL + " --collection " + CollectionName + " --out " + collResObj.MongoBackupPath + "%filename%\\ -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("echo MONGODUMP backup complete -  %date% - %time%");
                    sw.WriteLine("REM Check If Mongo dump backup file exists in folder");
                    sw.WriteLine("If Not Exist \"" + collResObj.MongoBackupPath + "%filename%\" (");
                    sw.WriteLine("    echo MONGODUMP backup does not exist \"" + collResObj.MongoBackupPath + "%filename%\" -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("REM Check If WINRAR exists in folder");
                    sw.WriteLine("If Not Exist \"" + collResObj.WinRARPath + "WINRAR.exe\" (");
                    sw.WriteLine("    echo WINRAR does not exist \"" + collResObj.WinRARPath + "WINRAR.exe\" -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("echo WINRAR started -  %date% - %time%");
                    sw.WriteLine("path=%path%;\"" + collResObj.WinRARPath + "\"");
                    sw.WriteLine("WinRAR.exe  a -ep1 -ibck \"" + collResObj.MongoBackupRARPath + "%filename%.zip\" \"" + collResObj.MongoBackupPath + "" + "%filename%\"");
                    sw.WriteLine("IF %ERRORLEVEL% NEQ 0 (");
                    sw.WriteLine("    echo WINRAR failed %errorlevel%. WinRAR.exe  a -ep1 -ibck \"" + collResObj.MongoBackupRARPath + "%filename%.zip\" \"" + collResObj.MongoBackupPath + "" + "%filename%\" -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("echo WINRAR complete -  %date% - %time%");
                    sw.WriteLine("REM Delete the backup directory (leave the ZIP file). The / q tag makes sure we don't get prompted for questions");
                    sw.WriteLine("echo BACKUP deleting original backup directory started %filename% -  %date% - %time%");
                    sw.WriteLine("rmdir \"" + collResObj.MongoBackupPath + "%filename%\" /s /q");
                    sw.WriteLine("IF %ERRORLEVEL% NEQ 0 (");
                    sw.WriteLine("    echo BACKUP deleting original backup directory failed %errorlevel%. \"" + collResObj.MongoBackupPath + "%filename%\" -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("echo BACKUP deleting original backup directory complete -  %date% - %time%");
                    sw.WriteLine("echo COMPLETE -  %date% - %time%");
                    sw.WriteLine("exit 100");
                }

                ProcessStartInfo processInfo;
                Process process;
                processInfo = new ProcessStartInfo("cmd.exe", "/c " + batFilePath + batFileName);
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;
                process = Process.Start(processInfo);
                process.WaitForExit();

                // *** Read the streams ***
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                var exitCode = process.ExitCode;
                if (exitCode != 0 || error != "")
                {
                    error = exitCode > 0 ? output : error;
                }

                if (exitCode == 100)
                {
                    if (File.Exists(batFilePath + batFileName))
                    {
                        File.Delete(batFilePath + batFileName);
                    }

                    db.DropCollection(CollectionName);
                    var command = new BsonDocument { { "repairDatabase", 1 } };
                    var stats = db.RunCommand<BsonDocument>(command);
                    GR.response_code = 0;
                    GR.response_message = "Backup Complete and Collection Dropped Successfully";
                    return GR;
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return GR;
        }

        public GenericResponse RestoreCollection(CollectionBackupAndRestore collResObj)
        {
            GenericResponse GR = new GenericResponse();
            try
            {
                GR.response_code = 1;

                Process proc = null;

                string batFilePath = collResObj.BatchFilePath;
                string batFileName = "UnzipAndRestore_" + collResObj.CollectionName + ".bat";

                if (File.Exists(batFilePath + batFileName))
                {
                    //File.Delete(batFilePath + batFileName);

                    GR.response_message = "Backup Already In Progress. Please wait.";
                    return GR;
                }

                using (FileStream fs = File.Create(batFilePath + batFileName))
                {
                    fs.Close();
                }

                string BackupFileName = "BACKUP_" + collResObj.DatabaseName + "_" + collResObj.CollectionName;

                if (!File.Exists(collResObj.MongoBackupRARPath + BackupFileName + ".zip"))
                {
                    GR.response_message = "Backup File Not Found";
                    return GR;
                }

                using (StreamWriter sw = new StreamWriter(batFilePath + batFileName))
                {
                    sw.WriteLine("@echo off");
                    sw.WriteLine("set logfilename=restore_archive_log_%date%");
                    sw.WriteLine("set logfilename=%logfilename:/=-%");
                    sw.WriteLine("set logfilename=%logfilename:/=-%");
                    sw.WriteLine("set logfilename=%logfilename: =__%");
                    sw.WriteLine("set logfilename=%logfilename:.=_%");
                    sw.WriteLine("set logfilename=%logfilename::=-%");
                    sw.WriteLine("call :sub >> " + collResObj.LogFilePath + "%logfilename%.txt");
                    sw.WriteLine("exit /b");
                    sw.WriteLine(":sub");
                    sw.WriteLine("echo ********************************************************************************************************************************");
                    sw.WriteLine("echo COLLECTION NAME : " + collResObj.CollectionName + "");
                    sw.WriteLine("set filename=" + BackupFileName + "");
                    sw.WriteLine("If Not Exist \"" + collResObj.MongoBackupRARPath + "%filename%.zip\" (");
                    sw.WriteLine("    echo WINRAR backup does not exist. \"" + collResObj.MongoBackupRARPath + "%filename%.zip\" -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("echo WINRAR started -  %date% - %time%");
                    sw.WriteLine("path=%path%;\"" + collResObj.WinRARPath + "\"");
                    sw.WriteLine("WinRAR.exe -ibck x \"" + collResObj.MongoBackupRARPath + "%filename%.zip\" *.* \"" + collResObj.MongoRestorePath + "\"");
                    sw.WriteLine("IF %ERRORLEVEL% NEQ 0 (");
                    sw.WriteLine("    echo WINRAR failed %errorlevel%. WinRAR.exe -ibck x \"" + collResObj.MongoBackupRARPath + "%filename%.zip\" *.* \"" + collResObj.MongoRestorePath + "\" -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("echo WINRAR complete -  %date% - %time%");
                    sw.WriteLine("If Not Exist \"" + collResObj.MongoRestorePath + "%filename%\" (");
                    sw.WriteLine("    echo MONGORESTORE backup does not exist. \"" + collResObj.MongoRestorePath + "%filename%\" -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("echo MONGORESTORE started -  %date% - %time%");
                    sw.WriteLine("path=%path%;\"" + collResObj.MongoServerPath + "\"");
                    sw.WriteLine("mongorestore --uri " + URL + " --db " + collResObj.DatabaseName + "  --collection " + collResObj.CollectionName + "  " + collResObj.MongoRestorePath + "%filename%\\" + collResObj.DatabaseName + "\\" + collResObj.CollectionName + ".bson");
                    sw.WriteLine("IF %ERRORLEVEL% NEQ 0 (");
                    sw.WriteLine("    echo MONGORESTORE failed %errorlevel%. mongorestore --uri " + URL + " --db " + collResObj.DatabaseName + "  --collection " + collResObj.CollectionName + "  " + collResObj.MongoRestorePath + "%filename%\\" + collResObj.DatabaseName + "\\" + collResObj.CollectionName + ".bson -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("echo MONGORESTORE complete -  %date% - %time%");
                    sw.WriteLine("echo BACKUP deleting original backup directory started %filename% -  %date% - %time%");
                    sw.WriteLine("rmdir \"" + collResObj.MongoRestorePath + "%filename%\" /s /q");
                    sw.WriteLine("IF %ERRORLEVEL% NEQ 0 (");
                    sw.WriteLine("    echo BACKUP deleting original backup directory failed %errorlevel%. \"" + collResObj.MongoRestorePath + "%filename%\"  -  %date% - %time%");
                    sw.WriteLine("    exit 919");
                    sw.WriteLine(")");
                    sw.WriteLine("echo BACKUP deleting original backup directory complete -  %date% - %time%");
                    sw.WriteLine("echo COMPLETE -  %date% - %time%");
                    sw.WriteLine("exit 100");
                }

                ProcessStartInfo processInfo;
                Process process;
                processInfo = new ProcessStartInfo("cmd.exe", "/c " + batFilePath + batFileName);
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;
                process = Process.Start(processInfo);
                process.WaitForExit();

                // *** Read the streams ***
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                var exitCode = process.ExitCode;
                if (exitCode != 0 || error != "")
                {
                    error = exitCode > 0 ? output : error;
                }

                if (exitCode == 100)
                {
                    if (File.Exists(batFilePath + batFileName))
                    {
                        File.Delete(batFilePath + batFileName);
                    }

                    GR.response_code = 0;
                    GR.response_message = "Collection Restored Successfully";
                    return GR;
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return GR;
        }

        public void ExportCollection(string DatabaseName, string CollectionName)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = MongoPath + "mongoexport.exe";
                startInfo.Arguments = "-d " + DatabaseName + " -c " + CollectionName + " --type csv --out " + ExportPath + "\\output.csv";
                startInfo.UseShellExecute = false;

                Process exportProcess = new Process();
                exportProcess.StartInfo = startInfo;

                exportProcess.Start();
                exportProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }

        public List<CollectionIndexList> GetAllCollectionIndexes(string DatabaseName, string CollectionName)
        {
            List<CollectionIndexList> indexList = new List<CollectionIndexList>();
            try
            {
                var db = mongoClient.GetDatabase(DatabaseName);
                var collection = db.GetCollection<BsonDocument>(CollectionName);
                using (var cursor = collection.Indexes.List())
                {
                    foreach (var document in cursor.ToEnumerable())
                    {
                        CollectionIndexList indexObj = new CollectionIndexList();
                        indexObj.IndexName = document.GetElement("name").Value.ToString();
                        var value = document.GetElement("key").Value;
                        var valueAsDocument = value.AsBsonDocument;
                        foreach (var elm in valueAsDocument.Elements)
                        {
                            indexObj.FieldName = elm.Name;
                        }
                        indexList.Add(indexObj);
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return indexList;
        }

        public List<Collection> SelectAllCollection(string DatabaseName)
        {
            var db = mongoClient.GetDatabase(DatabaseName);
            List<Collection> collectionList = new List<Collection>();
            Collection collectionObj = new Collection();
            try
            {
                using (var cursor = db.ListCollections())
                {
                    while (cursor.MoveNext())
                    {
                        foreach (var doc in cursor.Current)
                        {
                            collectionObj = new Collection();
                            collectionObj.CollectionName = doc["name"].ToString();
                            collectionList.Add(collectionObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
            return collectionList;
        }

        public void CreateIndex(string DatabaseName, string CollectionName, string FieldName, string IndexType)
        {
            try
            {
                var db = mongoClient.GetDatabase(DatabaseName);
                var collection = db.GetCollection<BsonDocument>(CollectionName);
                var indexDOC = new BsonDocument { { FieldName, 1 } };
                var indexModel = new CreateIndexModel<BsonDocument>(indexDOC);
                collection.Indexes.CreateOne(indexModel);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }

        public void DropIndex(string DatabaseName, string CollectionName, string IndexName)
        {
            try
            {
                var db = mongoClient.GetDatabase(DatabaseName);
                var collection = db.GetCollection<BsonDocument>(CollectionName);
                collection.Indexes.DropOne(IndexName);
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
                var db = mongoClient.GetDatabase(DatabaseName);
                var collection = db.GetCollection<BsonDocument>(CollectionName);
                var indexDOC = new BsonDocument { { FieldName, 1 } };
                var indexModel = new CreateIndexModel<BsonDocument>(indexDOC);
                await collection.Indexes.CreateOneAsync(indexModel).ConfigureAwait(false);
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
                var db = mongoClient.GetDatabase(DatabaseName);
                var collection = db.GetCollection<BsonDocument>(CollectionName);
                await collection.Indexes.DropOneAsync(IndexName);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin.DAL", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _localAppSettings);
            }
        }
    }
}