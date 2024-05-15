using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGenerateAPI.Entities.CustomModels
{
    public class DataInsertion
    {
        public string TableName { get; set; }
        public List<ColumnModel> Columns { get; set; }
    }

    public class ColumnModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}