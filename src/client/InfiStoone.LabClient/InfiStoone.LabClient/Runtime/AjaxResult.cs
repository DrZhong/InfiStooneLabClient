using Newtonsoft.Json;

namespace InfiStoone.LabClient.Runtime
{
    public class AjaxResultError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; } 
    }

    public class AjaxResult<T>
    {
        public bool Success { get; set; }       
        public bool UnAuthorizedRequest { get; set; }

        [JsonProperty("__abp")]
        public bool Abp { get; set; }   

        public AjaxResultError Error { get; set; }

        public T Result { get; set; }
    }

    public class AjaxResult:AjaxResult<object>
    { 
    }
}   