using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoAdmin.Models
{
    public class Document
    {
        public string Id { get; set; }
    }
    public class DocumentFields
    {
        public string FieldName { get; set; }
    }
    public class InsertDocument
    {
        public int SNo { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}
