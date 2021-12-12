using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mvvm
{
    public class Common
    {
        public class Result
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public long? ID { get; set; }
            public string Object { get; set; }
        }
    }
}