using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoAdmin.Models
{
    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<UserLoginMongoServer> AvailableServers { get; set; }
    }

    public class UserLoginMongoServer
    {
        public int MongoUserId { get; set; }
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
        public bool IsDefaultDB { get; set; }
    }
}
