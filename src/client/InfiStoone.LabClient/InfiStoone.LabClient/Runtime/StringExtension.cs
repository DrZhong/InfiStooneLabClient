using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Runtime
{
    public static class StringExtension
    {
        /// <summary>
        /// 把字符串转化成abp Result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static AjaxResult<T> StringToAjaxResult<T>(this string value)
        {
            return JsonConvert.DeserializeObject<AjaxResult<T>>(value);
        }

        /// <summary>
        /// 把String转化为 StringContent
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static StringContent StringToStringContent(this string value)
        {
            return new StringContent(value, Encoding.UTF8, "application/json");
        }
    }
}
