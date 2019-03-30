using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoAdmin.Models
{
    #region CommonClasses

    public class MongoCommand
    {
        public List<MongoParameter> Parameters { get; set; }

        public MongoCommand()
        {
            Parameters = new List<MongoParameter>();
        }
    }

    public class MongoParameter
    {
        public string name { get; set; }
        public dynamic value { get; set; }
        public bool matchExact { get; set; }
        public bool isFilterCondition { get; set; }
        public bool isSorted { get; set; }
        public bool isSortedAscorDesc { get; set; } //true -- Asc, false -- desc

        public MongoParameter(string parameterName, dynamic parameterValue)
        {
            name = parameterName;
            value = parameterValue;
            matchExact = false;
            isFilterCondition = false;
            isSorted = false;
            isSortedAscorDesc = false;
        }
        public MongoParameter(string parameterName, dynamic parameterValue, bool parameterMatchExactValue = true)
        {
            name = parameterName;
            value = parameterValue;
            matchExact = parameterMatchExactValue;
            isFilterCondition = false;
            isSorted = false;
            isSortedAscorDesc = false;
        }
        public MongoParameter(string parameterName, bool parameterIsSorted = false, bool parameterIsSortedAscorDesc = true)
        {
            name = parameterName;
            value = "";
            matchExact = false;
            isFilterCondition = false;
            isSorted = parameterIsSorted;
            isSortedAscorDesc = parameterIsSortedAscorDesc;
        }
        public MongoParameter(string parameterName, dynamic parameterValue, bool parameterMatchExactValue = true, bool parameterIsFilterCondition = false)
        {
            name = parameterName;
            value = parameterValue;
            matchExact = parameterMatchExactValue;
            isFilterCondition = parameterIsFilterCondition;
            isSorted = false;
            isSortedAscorDesc = false;
        }

        public MongoParameter(string parameterName, dynamic parameterValue, bool parameterMatchExactValue = true, bool parameterIsSorted = false, bool parameterIsSortedAscorDesc = true)
        {
            name = parameterName;
            value = parameterValue;
            matchExact = parameterMatchExactValue;
            isFilterCondition = false;
            isSorted = parameterIsSorted;
            isSortedAscorDesc = parameterIsSortedAscorDesc;
        }
    }

    #endregion CommonClasses
}
