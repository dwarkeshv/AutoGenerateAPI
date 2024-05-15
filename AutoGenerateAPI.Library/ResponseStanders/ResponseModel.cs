using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoGenerateAPI.Entities.ResponseStanders
{
    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; }

        [JsonIgnore]
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
