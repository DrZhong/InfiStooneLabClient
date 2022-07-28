using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.CodePrint.Dto
{
    public class ResultData<T>
    {
        public bool success { get; set; }

        public string error { get; set; }

        public T data { get; set; }
    }

    public class RequestData
    { 
        public string data { get; set; }
    }
}
