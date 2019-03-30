using System;
using System.Collections.Generic;

namespace MongoAdmin.Models
{
    public class AppConfigSettings
    {
        public string UserName { get; set; }
        public string MongoServer { get; set; }
        public string MongoPort { get; set; }
        public string MongoDBName { get; set; }
        public string MongoUserName { get; set; }
        public string MongoPassword { get; set; }
        public bool CreateAccess { get; set; }
        public bool ReadAccess { get; set; }
        public bool UpdateAccess { get; set; }
        public bool DeleteAccess { get; set; }
        public bool ExportAccess { get; set; }
        public string MongoServerPath { get; set; }
        public string ExportPath { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}