using InfiStoone.LabClient.Runtime.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Tools
{
    public class ConfigModel
    {
        public WareHouseDto WareHouse  { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 条码 串口
        /// </summary>
        public string BarCodePort { get; set; }
    }

    public static class Config
    {
        private static bool noCache = true;
        public static void BuildItems()
        {
            string path = "config.json";
            //HttpContext.Current.Server.MapPath("config.json")
            var json = File.ReadAllText(path);
            ConfigModel = JsonConvert.DeserializeObject<ConfigModel>(json);
        }

        public static ConfigModel ConfigModel { get; set; }

       

        /// <summary>
        /// 保存配置
        /// </summary>
        public static void SaveConfig()
        {
            File.WriteAllText("config.json", JsonConvert.SerializeObject(ConfigModel));
        } 
    }
}
