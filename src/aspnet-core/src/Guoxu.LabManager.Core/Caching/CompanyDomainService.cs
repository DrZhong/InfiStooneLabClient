using System.Collections.Generic;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Castle.DynamicProxy.Generators;
using Guoxu.LabManager.Caching.CacheItems;
using Guoxu.LabManager.Domains;

namespace Guoxu.LabManager.Caching;

public class CompanyDomainService:DomainService, IEventHandler<EntityChangedEventData<Company>>
{
    private const string CacheKey = "CompanyDomainService";
    private readonly IRepository<Company> _companyRepository;
    private readonly ICacheManager _cacheManager;
    private readonly UserCacheDomainService _userCacheDomainService;
    public CompanyDomainService(IRepository<Company> companyRepository,
        ICacheManager cacheManager, UserCacheDomainService userCacheDomainService)
    {
        _companyRepository = companyRepository;
        _cacheManager = cacheManager;
        _userCacheDomainService = userCacheDomainService;
    }

    public List<CompanyListDto> GetAllCompany(bool? isActive)
    {
        var userCache = _userCacheDomainService.GetCurrentUserCache();
        var list = _cacheManager.GetCache(CacheKey)
            .AsTyped<string, List<CompanyListDto>>()
            .Get(CacheKey+ userCache.CurrentSelectedWarehouseType?.ToString(), () =>
            {
                var query = _companyRepository.GetAll();
                var dto = this.ObjectMapper.ProjectTo<CompanyListDto>(query);
                return dto.ToList();
            });
        return list.WhereIf(isActive.HasValue, w => w.IsActive == isActive).ToList();
    }

    public void HandleEvent(EntityChangedEventData<Company> eventData)
    {
        _cacheManager.GetCache(CacheKey).Clear();
    }
}