using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoAdmin.Models
{
    public class ApplicationUser
    {
        public dynamic _id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<ApplicationMongoUser> MongoUser { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ValidTill { get; set; }
    }

    public class ApplicationMongoUser
    {
        public int MongoUserId { get; set; }
        public bool CreateAccess { get; set; }
        public bool ReadAccess { get; set; }
        public bool UpdateAccess { get; set; }
        public bool DeleteAccess { get; set; }
        public bool ExportAccess { get; set; }
        public bool IsDefaultDB { get; set; }
    }
}
