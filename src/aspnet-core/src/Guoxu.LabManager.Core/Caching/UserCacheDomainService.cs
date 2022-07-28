using Abp.Domain.Services;
using Abp.Runtime.Session;
using Abp.UI;
using Guoxu.LabManager.Caching.CacheItems;

namespace Guoxu.LabManager.Caching;

/// <summary>
/// 用户换成领域服务
/// </summary>
public class UserCacheDomainService:DomainService
{
    private readonly IUserCache _userCache;
    private readonly IAbpSession _abpSession;

    public UserCacheDomainService(IUserCache userCache, IAbpSession abpSession)
    {
        _userCache = userCache;
        _abpSession = abpSession;
    }

    /// <summary>
    /// 获取当前用户的缓存
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public UserCacheItem GetCurrentUserCache()
    {
        if (!_abpSession.UserId.HasValue)
        {
            throw new UserFriendlyException("当前用户没有登陆到系统！");
        }

        return _userCache[_abpSession.GetUserId()];
    }


    /// <summary>
    /// 根据用户id获取用户缓存
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public UserCacheItem GetUserCacheByUserId(long userId)
    {
        return _userCache[userId];
    }
}