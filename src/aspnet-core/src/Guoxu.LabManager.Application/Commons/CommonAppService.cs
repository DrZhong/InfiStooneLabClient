using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Abp.Application.Services;
using Abp.Auditing;
using Abp.Dapper.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Guoxu.LabManager.Authorization.Users;
using Guoxu.LabManager.BaseInfo.Dto;
using Guoxu.LabManager.Caching;
using Guoxu.LabManager.Caching.CacheItems;
using Guoxu.LabManager.Commons.Dto;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Domains.Repository;
using Guoxu.LabManager.Dto;
using Guoxu.LabManager.System.Dto;
using Guoxu.LabManager.Users.Dto; 

namespace Guoxu.LabManager.Commons;
[DisableAuditing]
[AbpAuthorize()]
public class CommonAppService: LabManagerAppServiceBase,

    IEventHandler<EntityChangedEventData<Dict>>
{
    private const string CacheKey = "CommonAppService";

    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly UserCacheDomainService _userCacheDomainService;
    private readonly CompanyDomainService _companyDomainService;
    private readonly ICacheManager _cacheManager;
    private readonly IRepository<Dict> _dictRepository;
    private readonly IRepository<Location> _locationRepository;
    private readonly IDapperRepository<ReagentStock> dapperRepository;
    private readonly IReagentStockDapperRepository _reagentStockDapperRepository;
    private readonly IRepository<User, long> _userRepository;
    public CommonAppService(IRepository<Warehouse> warehouseRepository,
        UserCacheDomainService userCacheDomainService,

        ICacheManager cacheManager,
        IRepository<Dict> dictRepository,
        CompanyDomainService companyDomainService,
        IRepository<Location> locationRepository,
        IDapperRepository<ReagentStock> dapperRepository,
        IReagentStockDapperRepository reagentStockDapperRepository, 
        IRepository<User, long> userRepository)
    {
        _warehouseRepository = warehouseRepository;
        _userCacheDomainService = userCacheDomainService;
        _cacheManager = cacheManager;
        _dictRepository = dictRepository;
        _companyDomainService = companyDomainService;
        _locationRepository = locationRepository;
        this.dapperRepository = dapperRepository;
        _reagentStockDapperRepository = reagentStockDapperRepository;
        _userRepository = userRepository;
    }


    /// <summary>
    /// 查询用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedResultDto<UserDto>> SearchUsers(PagedResultRequestFilterDto input)
    {
        var query = _userRepository.GetAll()
            .Where(w => w.IsActive)
            .WhereIf(!input.Filter.IsNullOrEmpty(),
                w => w.UserName.Contains(input.Filter) || w.Name.Contains(input.Filter));
        var count = await query.CountAsync();
        var list = await query.OrderBy(w => w.Id)
            .PageBy(input)
            .ToListAsync();
        return new PagedResultDto<UserDto>(count, ObjectMapper.Map<List<UserDto>>(list));
    }


    /// <summary>
    /// 获取所有可用的仓库
    /// </summary>  
    /// <returns></returns>
    [AbpAllowAnonymous]
    public async Task<List<WareHouseDto>> GetAllActiveWareHouse(bool? isActive)
    {
        var query = await _warehouseRepository
            .GetAll()
            .WhereIf(isActive.HasValue, w => w.IsActive == isActive.Value)
            .ToListAsync();
        return ObjectMapper.Map<List<WareHouseDto>>(query);
    }

    /// <summary>
    /// 获取所有可用的仓库
    /// </summary>  
    /// <returns></returns>
    public async Task<List<WareHouseDto>> GetMyWareActiveWareHouse(bool? isActive)
    {
        var userCace = _userCacheDomainService.GetCurrentUserCache();
        var query = await _warehouseRepository
            .GetAll()
            .Where(w=>w.WarehouseType==userCace.CurrentSelectedWarehouseType)
            .WhereIf(isActive.HasValue, w => w.IsActive == isActive.Value)
            .ToListAsync();
        return ObjectMapper.Map<List<WareHouseDto>>(query);
    }   

    /// <summary>
    /// 获取存储属性列表
    /// </summary>
    /// <returns></returns>
    public  List<EnumberEntityDto> GetStorageAttrList()
    {
        return GetStaticStorageAttrList();
    }

    public static List<EnumberEntityDto> GetStaticStorageAttrList()
    {
        List<EnumberEntityDto> list = new List<EnumberEntityDto>();

        foreach (var e in Enum.GetValues(typeof(StorageAttrEnum)))
        {
            EnumberEntityDto m = new EnumberEntityDto();
            object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (objArr != null && objArr.Length > 0)
            {
                DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                m.Desction = da.Description;
            }
            m.EnumValue = Convert.ToInt32(e);
            m.EnumName = e.ToString();
            list.Add(m);
        }
        return list;
    }


    #region 供应商
    /// <summary>
    /// 获取供应商
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public List<CompanyListDto> GetAllCompany(bool? isActive)
    {
       return _companyDomainService.GetAllCompany(isActive); 
    } 
    #endregion

    /// <summary>
    /// 获取纯度下拉
    /// </summary>
    /// <returns></returns> 
    public List<DictDto> GetPuritySelectList()
    {
        return this.GetAllDict().FirstOrDefault(w => w.Value == Dict.ReagentPurity)?.Child;
    }
    /// <summary>
    /// 获取容量单位
    /// </summary>
    /// <returns></returns>
    public List<DictDto> GetCapacityUnitSelectList()
    {
        return this.GetAllDict().FirstOrDefault(w => w.Value == Dict.ReagentCapacityUnit)?.Child;
    }
    /// <summary>
    /// 获取存储条件
    /// </summary>
    /// <returns></returns>
    public List<DictDto> GetStorageConditionSelectList()
    {
        var all = this.GetAllDict();
        var result= all.FirstOrDefault(w => w.Value == Dict.ReagentStorageCondition)?.Child;
        return result;
    }

    /// <summary>
    /// 获取所有库位
    /// </summary>
    /// <returns></returns>
    public async Task<List<LocationDto>> GetLocation()
    {
        var query =await _locationRepository.GetAllIncluding(w => w.LocationStorageAttr)
            .Where(w => w.IsActive).ToListAsync();

        return ObjectMapper.Map<List<LocationDto>>(query);
    }


    private List<DictDto> GetAllDict()
    {
        var userCache = _userCacheDomainService.GetCurrentUserCache();
        return _cacheManager.GetCache(CacheKey)
            .AsTyped<string, List<DictDto>>()
            .Get("GetAllDict-"+ userCache.CurrentSelectedWarehouseType?.ToString(), () =>
            {
                var query = this._dictRepository.GetAll().Include(w => w.Child).ToList();
                return this.ObjectMapper.Map<List<DictDto>>(query);
            });
    }
    [RemoteService(false)]
    public void HandleEvent(EntityChangedEventData<Dict> eventData)
    {
        _cacheManager.GetCache(CacheKey).Remove("GetAllDict-" + eventData.Entity.WarehouseType.ToString());
    }
     

    public async Task<HomeDto> HomeMasterDto()
    {
        var result=new HomeDto()
        {
            Master= await _reagentStockDapperRepository.GetHomeMaster(), 
        }; 

        return result;
    }

    public async Task<HomeNormalDto> HomeNormalDto()
    { 
        return await _reagentStockDapperRepository.GetHomeNormal();
    }
}