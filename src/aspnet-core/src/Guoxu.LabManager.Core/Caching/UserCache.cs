using Abp.Dependency;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Guoxu.LabManager.Authorization.Users;
using Guoxu.LabManager.Caching.CacheItems;

namespace Guoxu.LabManager.Caching;

public interface IUserCache : IEntityCache<UserCacheItem,long>
{

}

public class UserCache : EntityCache<User, UserCacheItem,long>, IUserCache, ITransientDependency
{
    public UserCache(ICacheManager cacheManager, IRepository<User, long> repository, IUnitOfWorkManager unitOfWorkManager, string cacheName = null) : base(cacheManager, repository, unitOfWorkManager, cacheName)
    {
    }
}