using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGenerateAPI.Entities.CustomModels
{
    public class TableCreation
    {
        public string? TableName { get; set; }
        public Dictionary<string, ColumnDetails> Columns { get; set; }
    }

    public class ColumnDetails
    {
        public string? DataType { get; set; } = "nvarchar";
        public string? MaxLength { get; set; } = "MAX";
        public string? choices { get; set; } = null;

    }
}
