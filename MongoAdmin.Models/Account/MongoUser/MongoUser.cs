using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoAdmin.Models
{
    public class MongoUser
    {
        public int _id { get; set; }
        public string MongoServer { get; set; }
        public string MongoPort { get; set; }
        public string MongoDBName { get; set; }
        public string MongoUserName { get; set; }
        public string MongoPassword { get; set; }
    }
}
