namespace MongoAdmin.Models
{
    public class Collection
    {
        public string CollectionName { get; set; }
    }

    public class CollectionStats
    {
        public string ns { get; set; }
        public double size { get; set; }
        public int count { get; set; }
        public int avgObjSize { get; set; }
        public int storageSize { get; set; }
        public bool capped { get; set; }
        public dynamic wiredTiger { get; set; }
        public int nindexes { get; set; }
        public dynamic indexDetails { get; set; }
        public int totalIndexSize { get; set; }
        public dynamic indexSizes { get; set; }
        public double ok { get; set; }
    }
    public class CollectionIndexList
    {
        public string IndexName { get; set; }

        public string FieldName { get; set; }
    }

    public class CollectionBackupAndRestore
    {
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public string MongoServerPath { get; set; }
        public string MongoBackupRARPath { get; set; }
        public string MongoBackupPath { get; set; }
        public string MongoRestorePath { get; set; }
        public string WinRARPath { get; set; }
        public string BatchFilePath { get; set; }
        public string LogFilePath { get; set; }
    }
}