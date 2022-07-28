using Abp.Dependency;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Guoxu.LabManager.Caching.CacheItems;
using Guoxu.LabManager.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Caching
{
    public interface ILocationCache : IEntityCache<LocationCacheItem, int>
    {

    }


    public class LocationCache
        : EntityCache<Location, LocationCacheItem>, ILocationCache, ITransientDependency
    {
        public LocationCache(ICacheManager cacheManager, IRepository<Location, int> repository, IUnitOfWorkManager unitOfWorkManager, string cacheName = null) : base(cacheManager, repository, unitOfWorkManager, cacheName)
        {
        }
    }
}
