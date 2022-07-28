using Abp.Dependency;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Storage
{
    public class TempFileCacheItem
    {
        public byte[] Bytes { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

    }
    public interface ITempFileCacheManager : ITransientDependency
    {
        void SetFile(string token, TempFileCacheItem content);

        TempFileCacheItem GetFile(string token);
    }
    public class TempFileCacheManager : ITempFileCacheManager
    {
        public const string TempFileCacheName = "TempFileCacheName";

        private readonly ICacheManager _cacheManager;

        public TempFileCacheManager(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void SetFile(string token, TempFileCacheItem content)
        {
            _cacheManager.GetCache(TempFileCacheName).Set(token, content, new TimeSpan(0, 0, 10, 0)); // expire time is 1 min by default
        }

        public TempFileCacheItem GetFile(string token)
        {
            var result = _cacheManager.GetCache(TempFileCacheName).Get(token, ep => ep) as TempFileCacheItem;
            return result;
        }
    }
}
