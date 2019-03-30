using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoAdmin.Models
{
    public class ServerStats
    {
        public string host { get; set; }
        public string version { get; set; }
        public string process { get; set; }
        public dynamic pid { get; set; }
        public double uptime { get; set; }
        public dynamic uptimeMillis { get; set; }
        public dynamic uptimeEstimate { get; set; }
        public dynamic localTime { get; set; }
        public dynamic asserts { get; set; }
        public dynamic connections { get; set; }
        public dynamic extra_info { get; set; }
        public dynamic globalLock { get; set; }
        public dynamic locks { get; set; }
        public dynamic logicalSessionRecordCache { get; set; }
        public dynamic network { get; set; }
        public dynamic opLatencies { get; set; }
        public dynamic opcounters { get; set; }
        public dynamic opcountersRepl { get; set; }
        public dynamic storageEngine { get; set; }
        public dynamic tcmalloc { get; set; }
        public dynamic wiredTiger { get; set; }
        public dynamic mem { get; set; }
        public dynamic metrics { get; set; }
        public double ok { get; set; }
    }
}
