using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MongoAdmin.DAL
{
    public class MongoProvider
    {
        public DataTable ToDataTable(List<IDictionary<string, object>> list)
        {
            DataTable result = new DataTable();
            if (list.Count == 0)
                return result;

            var columnNames = list.SelectMany(dict => dict.Keys).Distinct();
            result.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());
            foreach (Dictionary<string, object> item in list)
            {
                var row = result.NewRow();
                foreach (var key in item.Keys)
                {
                    if (item[key].GetType() == typeof(System.Object[]) && item[key].GetType().IsSerializable == true)
                        row[key] = JsonConvert.SerializeObject(item[key]);
                    else
                        row[key] = item[key];
                }

                result.Rows.Add(row);
            }
            return result;
        }
    }
}