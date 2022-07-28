using System;
using System.Collections.Generic;
using System.Linq; 
using Abp; 
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.Caching;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.ReagentService.Dto; 
using Guoxu.LabManager.Commons.Dto;
using Abp.Dapper.Repositories; 
using Abp.Runtime.Session;

namespace Guoxu.LabManager.ReagentService;

/// <summary>
/// 试剂服务
/// </summary>
[AbpAuthorize()]
public class ReagentStockAppService : LabManagerAppServiceBase
{
    private readonly IRepository<ReagentStock> _reagentStockRepository;
    private readonly IRepository<ReagentStockAudit> _reagentStockAuditRepository;
    private readonly IRepository<Reagent> _reagentRepository;
    private readonly IRepository<Location> _locationRepository;
    private readonly IRepository<ReagentLocation> _reagentLocationRepository;
    private readonly CompanyDomainService _companyDomainService;
    private readonly IRepository<ReagentStockHistory> _reagentStockHistoryRepository;
    private readonly IRepository<ReagentOperateRecord, long> _reagentOperateRecordRepository;
    private readonly IDapperRepository<ReagentStock> dapperRepository;
    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly ILocationCache _locationCache;

    public ReagentStockAppService(
        IRepository<Reagent> reagentRepository,
        IRepository<Location> locationRepository,
        IRepository<ReagentLocation> reagentLocationRepository,
        CompanyDomainService companyDomainService,
        IRepository<ReagentStock> reagentStockRepository,
        IRepository<ReagentStockHistory> reagentStockHistoryRepository,
        IRepository<ReagentOperateRecord, long> reagentOperateRecordRepository,
        IDapperRepository<ReagentStock> dapperRepository,
        IRepository<ReagentStockAudit> reagentStockAuditRepository,
        IRepository<Warehouse> warehouseRepository,
        ILocationCache locationCache)
    {
        _reagentRepository = reagentRepository;
        _locationRepository = locationRepository;
        _reagentLocationRepository = reagentLocationRepository;
        _companyDomainService = companyDomainService;
        _reagentStockRepository = reagentStockRepository;
        _reagentStockHistoryRepository = reagentStockHistoryRepository;
        _reagentOperateRecordRepository = reagentOperateRecordRepository;
        this.dapperRepository = dapperRepository;
        _reagentStockAuditRepository = reagentStockAuditRepository;
        _warehouseRepository = warehouseRepository;
        _locationCache = locationCache;
    }

    private string[] randomList = new[]
    {
        "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z","0","1","2"
        ,"3","4","5","6","7","8"
    };

    [AbpAuthorize(PermissionNames.Pages_Reagent_Ruku)]
    public async Task<PagedResultDto<ReagentStockListDto>> GetAllMaster(GetAllMasterInputDto input)
    {
        var query = _reagentStockRepository.GetAll()
            .WhereIf(!input.BarCode.IsNullOrEmpty(), w => w.BarCode == input.BarCode)
            .WhereIf(!input.ReagentNo.IsNullOrEmpty(), w => w.Reagent.No == input.ReagentNo)
            .WhereIf(!input.ReagentCasNo.IsNullOrEmpty(), w => w.Reagent.CasNo == input.ReagentCasNo)
            .WhereIf(!input.Filter.IsNullOrEmpty(), w => w.Reagent.CnName.Contains(input.Filter))
            .WhereIf(!input.BatchNo.IsNullOrEmpty(), w => w.BatchNo == input.BatchNo)
            .WhereIf(!input.SupplierCompanyName.IsNullOrEmpty(), w =>w.SupplierCompanyName.Contains(input.SupplierCompanyName))
            .WhereIf(input.WarehouseId.HasValue, w => w.WarehouseId == input.WarehouseId.Value)
            .WhereIf(input.StockStatus.HasValue, w => w.StockStatus == input.StockStatus);

        var count = await query.CountAsync();
        var list = query
            .Include(w => w.Warehouse)    //如果删除仓库 会导致数据查不全
            .Include(w => w.Reagent)
            .OrderBy(input.Sorting)
            .PageBy(input);
        //.ToListAsync();
        var dto = await this.ObjectMapper.ProjectTo<ReagentStockListDto>(list)
            .ToListAsync();
        return new PagedResultDto<ReagentStockListDto>(count, dto);
    }


    public async Task<LocationStockDto> GetLocationStock(int locationId)
    {
        var result = new LocationStockDto();
        var location = _locationCache[locationId];
        result.CountLimit = location.CountLimit;
        result.ExitStockCount=await _reagentStockRepository.CountAsync(w=>w.LocationId == locationId
        && w.StockStatus== ReagentStockStatusEnum.在库);
        result.OutStockCount = await _reagentStockRepository.CountAsync(w => w.LocationId == locationId
          && w.StockStatus == ReagentStockStatusEnum.离库);

        return result;
    }

        
    /// <summary>
    /// 获取双锁入库确认
    /// </summary>
    /// <param name="input"></param>
    /// <param name="doubleConfirmed"></param>
    /// <returns></returns>

    [AbpAuthorize(PermissionNames.Pages_Reagent_DoubleConfirm)]
    public async Task<PagedResultDto<ReagentStockListDto>> GetAllMasterDoubleConfirm(GetAllMasterInputDto input,bool doubleConfirmed)
    {
        var uid = AbpSession.GetUserId();
        var query = _reagentStockRepository.GetAll()
            .Where(w=>w.DoubleConfirm && w.DoubleConfirmed== doubleConfirmed)
            .WhereIf(!doubleConfirmed,w=>!w.ReagentStockAudits.Any(s=>s.AuditUserId==uid))
             .WhereIf(!input.BarCode.IsNullOrEmpty(), w => w.BarCode == input.BarCode)
            .WhereIf(!input.ReagentNo.IsNullOrEmpty(), w => w.Reagent.No == input.ReagentNo)
            .WhereIf(!input.ReagentCasNo.IsNullOrEmpty(), w => w.Reagent.CasNo == input.ReagentCasNo)
            .WhereIf(!input.Filter.IsNullOrEmpty(), w => w.Reagent.CnName.Contains(input.Filter))
            .WhereIf(!input.BatchNo.IsNullOrEmpty(), w => w.BatchNo == input.BatchNo)
            .WhereIf(!input.SupplierCompanyName.IsNullOrEmpty(), w => w.Reagent.SupplierCompanyName.Contains(input.SupplierCompanyName))
            .WhereIf(input.WarehouseId.HasValue, w => w.WarehouseId == input.WarehouseId.Value)
            .WhereIf(input.StockStatus.HasValue, w => w.StockStatus == input.StockStatus);

        var count = await query.CountAsync();
        var list = query
            .Include(w => w.Warehouse)    //如果删除仓库 会导致数据查不全
            .Include(w => w.Reagent)
            .OrderBy(input.Sorting)
            .PageBy(input);
        //.ToListAsync();
        var dto = await this.ObjectMapper.ProjectTo<ReagentStockListDto>(list)
            .ToListAsync();
        return new PagedResultDto<ReagentStockListDto>(count, dto);
    }

    /// <summary>
    /// 双锁确认
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AbpAuthorize(PermissionNames.Pages_Reagent_DoubleConfirm)]
    public async Task MasterDoubleConfirm(EntityDto input)
    {
        var entity = await _reagentStockRepository.GetAsync(input.Id);
        if (!entity.DoubleConfirm)
        {
            throw new UserFriendlyException("此试剂不需要双锁审核！");
        }
        if (entity.CreatorUserId == AbpSession.UserId)
        {
            throw new UserFriendlyException("你不能审核自己的入库单！");
        }

        var user = await GetCurrentUserAsync();
        //判断是否双锁过了
        var exit = await _reagentStockAuditRepository.GetAll()
            .AnyAsync(w => w.ReagentStockId == entity.Id
            && w.ReagentStockAuditType == ReagentStockAuditTypeEnum.入库双人双锁 &&
            w.CreatorUserId==user.Id);
        if (exit)
        {
            //说明已经审核过了
            throw new UserFriendlyException("您已经审核过此试剂了，不允许同一个人审核多次");
        }


        //双锁  
        await _reagentStockAuditRepository.InsertAsync(new ReagentStockAudit()
        {
            ReagentStockId=entity.Id,
            ReagentStockAuditType=ReagentStockAuditTypeEnum.入库双人双锁,
            AuditUserId=user.Id,
            AuditUserName=user.Name,
        });
        await CurrentUnitOfWork.SaveChangesAsync();
        //判断已锁是否超过2人

        var c=await _reagentStockAuditRepository.CountAsync(w=>w.ReagentStockId==input.Id
        && w.ReagentStockAuditType== ReagentStockAuditTypeEnum.入库双人双锁);
        if (c >= 2)
        {
            entity.DoubleConfirmed = true; 
        }

    }

    /// <summary>
    /// 获取终端确认
    /// </summary>
    /// <param name="input"></param>
    /// <param name="clientConfirmed"></param>
    /// <returns></returns>
    [AbpAuthorize(PermissionNames.Pages_Reagent_ClientConfirm)]
    public async Task<PagedResultDto<ReagentStockListDto>> GetAllMasterClientConfirm(GetAllMasterInputDto input, bool clientConfirmed)
    {
        var query = _reagentStockRepository.GetAll()
            .Where(w => w.ClientConfirm && w.ClientConfirmed == clientConfirmed)
             .WhereIf(!input.BarCode.IsNullOrEmpty(), w => w.BarCode == input.BarCode)
            .WhereIf(!input.ReagentNo.IsNullOrEmpty(), w => w.Reagent.No == input.ReagentNo)
            .WhereIf(!input.ReagentCasNo.IsNullOrEmpty(), w => w.Reagent.CasNo == input.ReagentCasNo)
            .WhereIf(!input.Filter.IsNullOrEmpty(), w => w.Reagent.CnName.Contains(input.Filter))
            .WhereIf(!input.BatchNo.IsNullOrEmpty(), w => w.BatchNo == input.BatchNo)
            .WhereIf(!input.SupplierCompanyName.IsNullOrEmpty(), w => w.Reagent.SupplierCompanyName.Contains(input.SupplierCompanyName))
            .WhereIf(input.WarehouseId.HasValue, w => w.WarehouseId == input.WarehouseId.Value)
            .WhereIf(input.StockStatus.HasValue, w => w.StockStatus == input.StockStatus);

        var count = await query.CountAsync();
        var list = query
            .Include(w => w.Warehouse)    //如果删除仓库 会导致数据查不全
            .Include(w => w.Reagent)
            .OrderBy(input.Sorting)
            .PageBy(input);
        //.ToListAsync();
        var dto = await this.ObjectMapper.ProjectTo<ReagentStockListDto>(list)
            .ToListAsync();
        return new PagedResultDto<ReagentStockListDto>(count, dto);
    }

    /// <summary>
    /// 由仓库管理员来确认
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AbpAuthorize(PermissionNames.Pages_Reagent_ClientConfirm)]
    public async Task MasterClientConfirm(EntityDto input)
    {
        var entity = await _reagentStockRepository.GetAsync(input.Id);
        if (!entity.ClientConfirm)
        {
            throw new UserFriendlyException("此试剂不需要 终端确认！");
        }
        if (entity.CreatorUserId == AbpSession.UserId)
        {
            throw new UserFriendlyException("你不能审核自己的入库单！");
        }
        var user = await GetCurrentUserAsync();
        //判断仓库管理员
        var house = await _warehouseRepository.GetAsync(entity.WarehouseId);
        if (house.MasterUserId.HasValue)
        {
            if (house.MasterUserId.Value != user.Id)
            {
                throw new UserFriendlyException(
                    0,
                    $"你不是仓库 {house.Name} 的管理员，无权审核此试剂入库！",
                    "如果你确定你是此仓库的管理员，可以联系系统管理员把你设置成此仓库的管理员后即可审核！"
                    ) ;
            }
        }
        
        
        //双锁  
        await _reagentStockAuditRepository.InsertAsync(new ReagentStockAudit()
        {
            ReagentStockId = entity.Id,
            ReagentStockAuditType = ReagentStockAuditTypeEnum.入库确认,
            AuditUserId = user.Id,
            AuditUserName = user.Name,
        });
        await CurrentUnitOfWork.SaveChangesAsync();
        //判断已锁是否超过2人

        entity.ClientConfirmed = true;
    }
    /// <summary>
    /// 获取审核人信息
    /// </summary>
    /// <param name="reagentStockId"></param>
    /// <returns></returns>
    [AbpAuthorize(PermissionNames.Pages_Reagent_Ruku)]
    public async Task<List<ReagentStockAuditDto>> GetReagentStockAudit(int reagentStockId)
    {
        var query=await _reagentStockAuditRepository.GetAllListAsync(w=>w.ReagentStockId==reagentStockId);
        return ObjectMapper.Map<List<ReagentStockAuditDto>>(query);
    }

    [AbpAuthorize(PermissionNames.Pages_Reagent_Ruku)]
    public List<string> GetCodes(int num)
    { 
        if (num > 200)
        {
            throw new UserFriendlyException("每次最多生成200个条码");
        }
        var result=new List<string>();
        for (int i = 0; i < num; i++)
        {
            result.Add(string.Join("", RandomHelper.GenerateRandomizedList(randomList).Take(10)));
        }

        return result;
    }

    [AbpAuthorize(PermissionNames.Pages_Reagent_Ruku)]
    public async Task<List<ReagentStockListDto>> CreateMasterStock(CreateReagentStockDto input)
    {
        var user = await GetCurrentUserAsync();
        if (!user.IsMaster)
        {
            throw new UserFriendlyException("只有专管才能创建专管库存！");
        }
        var reagent =await _reagentRepository.GetAsync(input.ReagentId);
        if (reagent.ReagentCatalog!=ReagentCatalog.专管试剂)
        {
            throw new UserFriendlyException("只有设置了专管试剂类型的试剂才能专管入库！");
        }
        var reagentLocation = await _reagentLocationRepository.FirstOrDefaultAsync(
            w => w.LocationId == input.LocationId && w.ReagentId == input.ReagentId);
        if (reagentLocation == null)
        {
            throw new UserFriendlyException($"选中的位置不能存储试剂 {reagent.CnName}！");
        }
        var location = await _locationRepository.GetAsync(input.LocationId);
        if (input.ProductionDate.HasValue)
        {
            //如果设置了生产日期，  就以生产日期加保质期 为过期日期！
            input.ExpirationDate = input.ProductionDate.Value.Date.AddMonths(input.ExpirationMonth);
        } 
        //校准条码是否唯一
        var exitCode=await _reagentStockRepository.GetAll()
            .FirstOrDefaultAsync(w=>input.Codes.Contains(w.BarCode));
        if (exitCode!=null)
        {
            throw new UserFriendlyException($"仓库中已经存在条码为 {exitCode.BarCode} 的试剂，请点击页面上的生成标签重新生成新的标签条码后再绑定！");
        }


        var entity = this.ObjectMapper.Map<ReagentStock>(input);
        entity.LocationName = location.Name;
        entity.CreateUserName = user.Name;
        entity.WarehouseId = location.WarehouseId;
        entity.Capacity = reagent.Capacity;
        entity.CapacityUnit = reagent.CapacityUnit;
        entity.PinYinCode=reagent.PinYinCode;
        entity.CasNo = reagent.CasNo;
        entity.SafeAttribute = reagent.SafeAttribute;
        entity.ClientConfirm = reagent.ClientConfirm; //input.ClientConfirm;
        entity.DoubleConfirm = reagent.DoubleConfirm; //input.DoubleConfirm;
        if (input.Price > 0)
        {
            entity.Price = input.Price;
        }
        else
        {
            entity.Price = reagent.Price;
        }
        
        var companies = _companyDomainService.GetAllCompany(null);
        if (input.SupplierCompanyId.HasValue)
        {
            var supplierCompany = companies.FirstOrDefault(w => w.Id == input.SupplierCompanyId);
            entity.SupplierCompanyName = supplierCompany?.Name;
        }

        if (input.ProductionCompanyId.HasValue)
        {
            var productionCompany = companies.FirstOrDefault(w => w.Id == input.ProductionCompanyId);
            entity.ProductionCompanyName = productionCompany?.Name;
        }

        entity.StockStatus = ReagentStockStatusEnum.待入库;

        List<ReagentStockListDto> insertList = new List<ReagentStockListDto>();
        for (int i = 0; i < input.Codes.Count; i++)
        {
            var newEntity = this.ObjectMapper.Map<ReagentStock>(entity);
            newEntity.BarCode = input.Codes[i]; //string.Join("", RandomHelper.GenerateRandomizedList(randomList).Take(10));
            await _reagentStockRepository.InsertAndGetIdAsync(newEntity);
            insertList.Add(this.ObjectMapper.Map<ReagentStockListDto>(newEntity)); 
        } 
        await CurrentUnitOfWork.SaveChangesAsync();
        return insertList;
    }

    [AbpAuthorize(PermissionNames.Pages_Reagent_Ruku)]
    public async Task DeleteMasterStock(EntityDto input)
    {
        var user = await GetCurrentUserAsync();
        if (!user.IsMaster)
        {
            throw new UserFriendlyException("只有专管才能删除专管库存！");
        }
        var entity=await this._reagentStockRepository.GetAsync(input.Id);
        if (entity.StockStatus != ReagentStockStatusEnum.待入库)
        {
            throw new UserFriendlyException("此试剂已入库，无法删除！");
        }

        await _reagentStockRepository.DeleteAsync(entity);
    }

  

    /// <summary>
    /// 回收试剂
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AbpAuthorize(PermissionNames.Pages_Reagent_Ruku_HuiShou)]
    public async Task BackMasterStock(EntityDto input)
    {
        var user = await GetCurrentUserAsync();
        if (!user.IsMaster)
        {
            throw new UserFriendlyException("只有专管才能回收专管库存！");
        }
        var entity = await this._reagentStockRepository.GetAsync(input.Id);
        if(entity.StockStatus!= ReagentStockStatusEnum.在库)
        {
            throw new UserFriendlyException("只有 在库  的试剂才允许回收");
        }
        //if (entity.StockStatus == ReagentStockStatusEnum.在库
        //    || entity.StockStatus == ReagentStockStatusEnum.离库)
        //{

        //}
        //else
        //{
        //    throw new UserFriendlyException("只有 在库 和 离库 的试剂才允许回收");
        //}

        entity.StockStatus = ReagentStockStatusEnum.已用完;
        entity.RetrieveTime=DateTime.Now;
        entity.RetrieveUserName = user.Name;
        await _reagentStockHistoryRepository.InsertAsync(new ReagentStockHistory()
        {
            ReagentStockId = entity.Id,
            Content = $"试剂 回收",
            CreateUserName = user.Name
        });

        await _reagentOperateRecordRepository.InsertAsync(new ReagentOperateRecord()
        {
            LocationId = entity.LocationId,
            BarCode = entity.BarCode,
            LocationName = entity.LocationName,
            WarehouseId = entity.WarehouseId,
            Capacity = entity.Capacity,
            CapacityUnit = entity.CapacityUnit,
            BatchNo = entity.BatchNo,
            SafeAttribute = entity.SafeAttribute,
            OperateType = OperateTypeEnum.回收,
            ReagentId = entity.ReagentId,
            CreateUserName = user.Name,
            Year = DateTime.Now.Year,
            Month = DateTime.Now.Month,
            Day = DateTime.Now.Day,
            Hour = DateTime.Now.Hour,
            Minute = DateTime.Now.Minute
        });
    }

    /// <summary>
    /// 专管操作记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AbpAuthorize(PermissionNames.Pages_Reagent_CaozuoJilu)]
    public async Task<PagedResultDto<ReagentOperateRecordDto>> GetMasterReagentOperateRecord(ReagentOperateRecordDtoInputDto input)
    {
        var query = _reagentOperateRecordRepository.GetAll()
            .WhereIf(!input.ReagentNo.IsNullOrEmpty(), w => w.Reagent.No.Contains(input.ReagentNo))
            .WhereIf(!input.Filter.IsNullOrEmpty(),
                w => w.Reagent.CnName.Contains(input.Filter) || w.Reagent.CnAliasName.Contains(input.Filter))
            .WhereIf(!input.BarCode.IsNullOrEmpty(), w => EF.Functions.Like(w.BarCode, $"%{input.BarCode}%"))
            .WhereIf(!input.CreateUserName.IsNullOrEmpty(),
                w => EF.Functions.Like(w.CreateUserName, $"%{input.CreateUserName}%"))
            .WhereIf(input.StartDate.HasValue, w => w.CreationTime >= input.StartDate)
            .WhereIf(input.EndDate.HasValue, w => w.CreationTime <= input.EndDate)
            .WhereIf(input.OperateType.HasValue, w => w.OperateType == input.OperateType)
            .WhereIf(!input.CasNo.IsNullOrEmpty(), w => w.Reagent.CasNo.Contains(input.CasNo))
            .WhereIf(input.WarehouseId.HasValue, w => w.WarehouseId == input.WarehouseId);
        var count = await query.CountAsync();
        var list = await query
            .Include(w=>w.Reagent)
            .Include(w=>w.Warehouse)
            .OrderBy(input.Sorting)
            .PageBy(input)
            .ToListAsync();
        var dto = this.ObjectMapper.Map<List<ReagentOperateRecordDto>>(list);
        return new PagedResultDto<ReagentOperateRecordDto>(count, dto);

    }

    /// <summary>
    /// 1 濒临过期
    /// 2 已过期
    /// 3 库存紧张
    /// </summary>
    string GetSql(int stockStatus)
    {
        if(stockStatus == 1)
        {
            return " and DATEDIFF(day,GETDATE(),ExpirationDate) <30 and  DATEDIFF(day,GETDATE(),ExpirationDate)>0";
        }
        else
        {
            return " and DATEDIFF(day,GETDATE(),ExpirationDate) <0";
        }
    }


    /// <summary>
    /// 专管库存查询
    /// </summary>
    /// <returns></returns>
    [AbpAuthorize(PermissionNames.Pages_Reagent_KucunChaxun)]
    public async Task<PagedResultDto<ClientStockDto>> GetMasterReagentStock(GetMasterReagentStockInputDto input)
    {
        input.MaxResultCount++;
        
        string sql = $@"select a.Num,a.minPrice,a.maxPrice,a.TotalWeight,w.[Name] as WarehouseName,b.* from
(select ReagentId,WarehouseId,count(1) as Num,min(price) as minPrice,sum(Weight) as TotalWeight,max(price) as maxPrice from [dbo].[ReagentStock] where StockStatus=1 and   isdeleted=0
{(input.WarehouseId.HasValue? " and WarehouseId=@WarehouseId":"")}
   
{(input.StockStatus.HasValue&&input.StockStatus.Value<3? GetSql(input.StockStatus.Value) : "")}
group by ReagentId,WarehouseId
) a
inner join [dbo].[Reagent]  b
on a.ReagentId =b.Id
{(input.No.IsNullOrEmpty()?"": " and b.No=@No")}
{(input.CasNo.IsNullOrEmpty() ? "" : " and b.CasNo=@CasNo")}
{(input.Filter.IsNullOrEmpty() ? "" : " and b.CnName like '%'+@Filter+'%'")}
{(input.Purity.IsNullOrEmpty() ? "" : " and b.Purity=@Purity")}
{(input.StorageCondition.IsNullOrEmpty() ? "" : " and b.StorageCondition=@StorageCondition")}
{(input.StockStatus.HasValue && input.StockStatus.Value == 3 ? " and (b.InventoryWarning>0 and b.InventoryWarning>=a.Num)" : "")}
inner join Warehouse w on a.WarehouseId=w.Id
order by ReagentId
offset @SkipCount rows fetch next @MaxResultCount rows only ";
        var query =await dapperRepository.QueryAsync<ClientStockDto>(sql, input);

        return new PagedResultDto<ClientStockDto>(
            input.SkipCount+ query.Count(), 
            query.Take(input.MaxResultCount--)
            .ToList());
    }

    /// <summary>
    /// 通过试剂编号获取试剂列表
    /// </summary>  
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedResultDto<ReagentStockDto>> GetMasterReagentStockDetailByNo(GetMasterReagentStockDetailByNoInput input)
    {
        var query = this._reagentStockRepository
            .GetAll()
            .Where(w => w.StockStatus==ReagentStockStatusEnum.在库 && w.Reagent.No == input.RegentNo);
        if (input.StockStatus.HasValue)
        {
            switch (input.StockStatus.Value)
            {
                case 1:
                    var duibi=DateTime.Now.AddDays(30);
                    query = query.Where(q => q.ExpirationDate <duibi && q.ExpirationDate>DateTime.Now);
                    break;
                case 2:
                    query=query.Where(q=>q.ExpirationDate < DateTime.Now);
                    break;
                case 3:
                    break;
            }
        } 
        var count=await query.CountAsync();
        var list = await query
            .Include(w => w.Warehouse)
            .Include(w => w.Reagent)
            .OrderBy(w => w.ExpirationDate)
            .PageBy(input)
            .ToListAsync();
        return new PagedResultDto<ReagentStockDto>(count, ObjectMapper.Map<List<ReagentStockDto>>(list));
    }


 
    [AbpAuthorize(PermissionNames.Pages_Reagent_Tongji)]
    public async Task<GerChartDataOutDto> GerChartData(GerChartDataIntputDto input)
    {
        if (input.GroupBy == 0)
        {
            //不要超过24小时
            if ((input.EndDate.Value - input.StartDate.Value).TotalHours > 24)
            {
                throw new UserFriendlyException("按时统计，时间间隔不得超过24个小时!");
            }
        }
        string sql = $@"  select　
 {(GetGroupBySelect(input.GroupBy))},
  OperateType,
  count(1) as num
   from ReagentOperateRecord 
where CreationTime >@StartDate 
and CreationTime< @EndDate
{(input.ReagentId.HasValue? " and ReagentId=@ReagentId":"")}
{(input.SafeAttributes.HasValue? " AND SafeAttribute=@SafeAttributes" : "")}
   group by
  {(GetGroupBy(input.GroupBy))}, 
OperateType";
        var result=new GerChartDataOutDto(); 
        var query = await dapperRepository.QueryAsync<TempGerChartDataClass>(sql, input);//.ToListAsync();

        result.Time=query.GroupBy(w => w.Time)
            .Select(w => w.Key)
            .OrderBy(w=>w).ToList();  
        foreach (var x in result.Time)
        {
            result.StockIn.Add(query.FirstOrDefault(w => w.Time == x && w.OperateType == OperateTypeEnum.入库)?.Num ?? 0);
            result.StockOut.Add(query.FirstOrDefault(w => w.Time == x && w.OperateType == OperateTypeEnum.领用)?.Num ?? 0);
            result.StockBack.Add(query.FirstOrDefault(w => w.Time == x && w.OperateType == OperateTypeEnum.归还)?.Num ?? 0);
            result.StockRetrieve.Add(query.FirstOrDefault(w => w.Time == x && w.OperateType == OperateTypeEnum.回收)?.Num ?? 0);
        }
        return result;
    }
    /// <summary>
    ///  0-按时，1-按日，2-按周，3-按月
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    private string GetGroupBy(int g)
    {
        switch (g)
        {
            case 0: return " [Hour]";
            case 1: return " [Year],[Month],[Day]";
            case 2: return " [Year],[Week]";
            case 3: return " [Year],[Month]";
        }
        return " [Year],[Month],[Day]";
    }
    /// <summary>
    ///  0-按时，1-按日，2-按周，3-按月
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    private string GetGroupBySelect(int g)
    {
        switch (g)
        {
            case 0: return " RIGHT('0'+cast([Hour] as varchar),2) as [Time]";
            case 1: return " cast([Year] as varchar)+'-'+RIGHT('0'+cast([Month] as varchar),2)+'-'+RIGHT('0'+cast([Day] as varchar),2) as [Time]";
            case 2: return " cast([Year] as varchar)+'-'+RIGHT('0'+cast([Week] as varchar),2) as [Time]";
            case 3: return " cast([Year] as varchar)+'-'+RIGHT('0'+cast([Month] as varchar),2) as [Time]";
        }
        return " cast([Year] as varchar)+'-'+RIGHT('0'+cast([Month] as varchar),2)+'-'+RIGHT('0'+cast([Day] as varchar),2) as [Time]";
    }
}