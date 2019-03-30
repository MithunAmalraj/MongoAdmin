namespace MongoAdmin.Models
{
    public class DatabaseStats
    {
        public string db { get; set; }
        public int collections { get; set; }
        public int views { get; set; }
        public int objects { get; set; }
        public double avgObjSize { get; set; }
        public double dataSize { get; set; }
        public double storageSize { get; set; }
        public int numExtents { get; set; }
        public int indexes { get; set; }
        public double indexSize { get; set; }
        public double fsUsedSize { get; set; }
        public double fsTotalSize { get; set; }
        public double ok { get; set; }
    }
    public class Database
    {
        public string DatabaseName { get; set; }
        public string SizeOnDisk { get; set; }
    }
}