using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Guoxu.LabManager.Authorization.Users;
using Guoxu.LabManager.Domains;

namespace Guoxu.LabManager.Caching.CacheItems;

[AutoMapFrom(typeof(User))]
public class UserCacheItem:EntityDto<long>
{
    public string Name { get; set; }    

    /// <summary>
    /// 当前用户选中的登陆仓库类型
    /// </summary> 
    public WarehouseType? CurrentSelectedWarehouseType { get; set; }

    /// <summary>
    /// 是否是超管
    /// </summary>
    public bool IsMaster { get; set; } = false;
}