using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Tools
{
    public static class Utll
    {
        public const string SaltKey = "InfiStoone.LabClient.Pages.EnterLucene";
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }


        public static bool CheckLucense()
        {
            return true;
            var cinfigModel = Config.ConfigModel;
             
            //校验密钥
            var result = Utll.GetMacAddress();
            if (result.Count == 0)
            {
                return false;
            }

            foreach (var item in result)
            {
                string targetKey = item + Utll.SaltKey;
                var md5Key = Utll.GenerateMD5(targetKey);
                if (cinfigModel.Token.Equals(md5Key,StringComparison.OrdinalIgnoreCase))
                {
                    return true; 
                }
            }
            return false;
        }



        public static List<string> GetMacAddress()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces().ToList();

            return interfaces.Select(w => BitConverter.ToString(w.GetPhysicalAddress().GetAddressBytes()))
                .Where(w => !string.IsNullOrEmpty(w))
                .ToList();
        }
    }
}
