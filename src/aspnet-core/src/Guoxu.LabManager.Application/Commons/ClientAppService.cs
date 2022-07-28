using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.Authorization.Users;
using Guoxu.LabManager.BaseInfo.Dto;
using Guoxu.LabManager.Caching;
using Guoxu.LabManager.Commons.Dto;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.DomainServices;
using Guoxu.LabManager.ReagentService.Dto;
using Guoxu.LabManager.Roles.Dto;
using Guoxu.LabManager.Users.Dto;
using Microsoft.EntityFrameworkCore;

namespace Guoxu.LabManager.Commons;

/// <summary>
/// 客户端服务
/// </summary>
[AbpAuthorize()]
public class ClientAppService: LabManagerAppServiceBase
{
    private readonly IRepository<WarehousePermission> _warehousePermissionRepository; 
    private readonly IRepository<ReagentStock> _reagentStockRepository; 
    private readonly IRepository<ReagentStockHistory> _reagentStockHistoryRepository;
    private readonly IRepository<Warehouse> _warehouseRepository; 
    private readonly IRepository<ReagentOperateRecord, long> _reagentOperateRecordRepository;
    private readonly IRepository<UserRole, long> _userRoleRepository;   
    private readonly IDapperRepository<ReagentStock> dapperRepository;  
    private readonly IRepository<NormalReagentStock> _normalReagentStockRepository;
    private readonly IRepository<NormalReagentOperateRecord,long> _normalReagentOperateRecordRepository;
    private readonly IRepository<OutOrder> _outOrderRepository;
    private readonly OutOrderDomainService _outOrderDomainService;
    private readonly IRepository<UserFinger> _userFingerRepository;
    private readonly ILocationCache _locationCache;

    public ClientAppService(
        IRepository<ReagentStock> reagentStockRepository,
        IRepository<Warehouse> warehouseRepository,
        IRepository<ReagentStockHistory> reagentStockHistoryRepository,
        IRepository<ReagentOperateRecord, long> reagentOperateRecordRepository,
        IRepository<WarehousePermission> warehousePermissionRepository,
        IRepository<UserRole, long> userRoleRepository,
        IDapperRepository<ReagentStock> dapperRepository,
        IRepository<NormalReagentStock> normalReagentStockRepository,
        IRepository<NormalReagentOperateRecord, long> normalReagentOperateRecordRepository,
        IRepository<OutOrder> outOrderRepository, OutOrderDomainService outOrderDomainService,
        ILocationCache locationCache, IRepository<UserFinger> userFingerRepository)
    {
        _reagentStockRepository = reagentStockRepository;
        _warehouseRepository = warehouseRepository;
        _reagentStockHistoryRepository = reagentStockHistoryRepository;
        _reagentOperateRecordRepository = reagentOperateRecordRepository;
        _warehousePermissionRepository = warehousePermissionRepository;
        _userRoleRepository = userRoleRepository;
        this.dapperRepository = dapperRepository;
        _normalReagentStockRepository = normalReagentStockRepository;
        _normalReagentOperateRecordRepository = normalReagentOperateRecordRepository;
        _outOrderRepository = outOrderRepository;
        _outOrderDomainService = outOrderDomainService;
        _locationCache = locationCache;
        _userFingerRepository = userFingerRepository;
    }

    /// <summary>
    /// 获取所有指纹
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserFingerDto>> GetAllUserFinger()
    {
        var query=await _userFingerRepository.GetAll()
            .Include(w=>w.User)
            .Where(w=>w.User.IsActive).ToListAsync();
        return ObjectMapper.Map<List<UserFingerDto>>(query);
    }

    [AbpAuthorize(PermissionNames.Pages_Administrator_Users)]
    public async Task AddUserFinger(UserFingerDto input)
    {
        var entity = new UserFinger()
        {
            UserId = input.UserId,
            Data1 = input.Data1,
            Data2 = input.Data2,
            Data3 = input.Data3,
            Data4 = input.Data4,
            Data5 = input.Data5,
            Data6 = input.Data6,
        };
        await _userFingerRepository.InsertAsync(entity);
    }


    /// <summary>
    /// 根据仓库ID获取对应用户所拥有的权限
    /// </summary>
    /// <param name="wareHouseId"></param>
    /// <returns></returns>
    public async Task<List<WarehousePermissionDto>> GetUserWarehousePermission(int wareHouseId)
    { 
        var roleIds= _userRoleRepository.GetAll().Where(w=>w.UserId==AbpSession.UserId)
            .Select(w=>w.RoleId).ToList();
        var query= await _warehousePermissionRepository.GetAllListAsync(w=>w.WarehouseId==wareHouseId
        &&w.IsActive==true
        &&roleIds.Contains(w.RoleId));

        return this.ObjectMapper.Map<List<WarehousePermissionDto>>(query);
    }
    /// <summary>
    /// 查库存
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="warehouseId"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async  Task<List<ClientStockDto>> SearchStock(string filter,int warehouseId, int pageSize = 20)
    {
        string sql = $@"  select 
  a.*,b.*
  from
  (select  top {pageSize}
  ReagentId,LocationName,count(1) as num,min(price) as minPrice,max(price) as maxPrice
  from [ReagentStock] where IsDeleted=0  and StockStatus={(int)ReagentStockStatusEnum.在库} and WarehouseId={warehouseId} and (PinYinCode like '%'+@filter+'%' or CasNo=@filter) 
  group by ReagentId,LocationName) a
  inner join  Reagent b on a.ReagentId=b.Id";

        var result= dapperRepository.Query<ClientStockDto>(sql, new { filter });
        foreach (var item in result)
        {
            item.IsMaster = true;
        }
        if (result.Count() > 0)
        {
            return result.ToList();
        }
        //如果在专管没有查到 那么就查普通的
        var query=await _normalReagentStockRepository
            .GetAll()
            //.Where(w=>w.WarehouseId==warehouseId && w.RealAmount>0)
            .Where(w => w.WarehouseId == warehouseId)
            .Include(w=>w.Reagent)
            .Where(w=>EF.Functions.Like(w.PinYinCode,$"%{filter}%")||w.CasNo==filter)
            .ToListAsync();

        return query.Select(w => new ClientStockDto()
        {
            IsMaster = false,
            LocationName=w.LocationName,
            BarCode=w.BarCode,
            Num=w.RealAmount, 
            Price=w.Price,
            MinPrice=w.Price,
            MaxPrice=w.Price,
            Capacity=w.Capacity,
            CapacityUnit=w.CapacityUnit,
            Purity =w.Reagent.Purity,
            CnName=w.Reagent.CnName,
            SupplierCompanyName=w.SupplierCompanyName,
            CreateUserName=w.CreateUserName,
        }).ToList();
    }



    /// <summary>
    /// 查库存  精准搜索
    /// </summary>
    /// <param name="code"></param>
    /// <param name="warehouseId"></param> 
    /// <returns></returns>
    public async Task<List<ClientStockDto>> SearchStockByCode(string code, int warehouseId )
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code));
        }
        string sql = $@"select a.*,b.* from [ReagentStock] a 
   inner join  Reagent b on a.ReagentId=b.Id
where a.IsDeleted=0 and a.StockStatus={(int)ReagentStockStatusEnum.在库} and a.WarehouseId={warehouseId}   and a.BarCode=@code ";

        var result = dapperRepository.Query<ClientStockDto>(sql, new { code });
        foreach (var item in result)
        {
            item.IsMaster = true;
            item.Num = 1;
        }
        if (result.Any())
        { 
            return result.ToList();
        }
        //否则查找普通试剂
        var query=await _normalReagentStockRepository
            .GetAll().Include(w=>w.Reagent)
            .FirstOrDefaultAsync(w=>w.BarCode == code && w.WarehouseId==warehouseId);
        if(query == null)
        {
            //如果每招到
            throw new UserFriendlyException($"没有找到 {code} 相关的库存，确定此试剂是否在此仓库中！");
        }
        var dto= ObjectMapper.Map<ClientStockDto>(query);
        dto.IsMaster = false;
        dto.CnName = query.Reagent.CnName;
        dto.Num = query.RealAmount;
        return new List<ClientStockDto>
        {
            dto
        };

    }

    /// <summary>
    /// 出库
    /// </summary>
    /// <returns></returns>
    public async Task StockOut(StockOutInputDto input)
    {
        var mastStockOut=await _reagentStockRepository.FirstOrDefaultAsync(w=>w.BarCode == input.BarCode&&
        w.WarehouseId==input.WarehouseId &&
        w.StockStatus== ReagentStockStatusEnum.在库);
        if (mastStockOut != null)
        {
            await MasterOut(mastStockOut, true);
        }
        else
        {
            var normoal = await _normalReagentStockRepository
                .GetAll()
                .Include(w => w.Reagent)
                .FirstOrDefaultAsync(w => w.BarCode == input.BarCode
                && w.WarehouseId == input.WarehouseId);
            if (normoal == null)
            {
                throw new UserFriendlyException($"未找到条码为 {input.BarCode} 相关的库存信息");
            }
            if (input.Num > normoal.RealAmount)
            {
                throw new UserFriendlyException($"此试剂在库数量只有：{normoal.RealAmount} 瓶，无法出库 {input.Num} 瓶");
            }
            //可以出库咯
            var currentUserName = (await GetCurrentUserAsync()).Name;
            await CommonOut(normoal,input.Num, currentUserName);

        }
    }

    private async Task CommonOut(NormalReagentStock normoal,int stockOutNum,string createUserName)
    {
        normoal.RealAmount -= stockOutNum; //input.Num;//减库存
        normoal.LatestStockOutTime = DateTime.Now;
        normoal.LatestStockOutUserName = createUserName;//(await GetCurrentUserAsync()).Name;
        await _normalReagentStockRepository.UpdateAsync(normoal);
        var wh = await _warehouseRepository.GetAsync(normoal.WarehouseId);
        //日志
        await _normalReagentOperateRecordRepository.InsertAsync(new NormalReagentOperateRecord()
        {
            NormalReagentStockId = normoal.Id,
            BarCode = normoal.BarCode,
            LocationId = normoal.LocationId,
            LocationName = normoal.LocationName,
            WarehouseId = normoal.WarehouseId,
            WarehouseName = wh.Name,
            Capacity = normoal.Capacity,
            CapacityUnit = normoal.CapacityUnit,
            OperateAmount = stockOutNum,// input.Num,
            //SafeAttribute = entity.SafeAttribute,
            BatchNo = normoal.BatchNo,
            OperateType = OperateTypeEnum.领用,
            ReagentId = normoal.ReagentId,
            ReagentNo = normoal.Reagent.No,
            ReagentCasNo = normoal.Reagent.CasNo,
            ReagentCnName = normoal.Reagent.CnName,
            ReagentEnName = normoal.Reagent.EnName,
            ReagentReagentCatalog = normoal.Reagent.ReagentCatalog,
            ReagentPinYinCode = normoal.Reagent.PinYinCode,
            CreateUserName =  createUserName,
            Year = DateTime.Now.Year,
            Month = DateTime.Now.Month,
            Day = DateTime.Now.Day,
            Hour = DateTime.Now.Hour,
            Minute = DateTime.Now.Minute
        });
    }

    private async Task MasterOut(ReagentStock mastStockOut,bool checkConfirm )
    {
        if (mastStockOut.LockedOrderId.HasValue)
        {
            //说明此试剂正在被锁定，无法出库哦
            throw new UserFriendlyException($"此试剂正在被出库单 {mastStockOut.LockedOrderId.Value.ToString()} 锁定，你无法出库！");
        }
        if (checkConfirm)
        {
            //是否审核通过
            if (mastStockOut.ClientConfirm)
            {
                throw new UserFriendlyException("此试剂需要  仓库管理员确认才能领用，请通过发起出库单审核流程出库！");

            }
            if (mastStockOut.DoubleConfirm)
            {
                throw new UserFriendlyException("此试剂需要  双锁才能领用，请通过发起出库单审核流程出库！");
            }
        }  
        var user = await GetCurrentUserAsync();
        mastStockOut.LatestStockOutTime = DateTime.Now; ;
        mastStockOut.LatestStockOutUserName = user.Name;
        mastStockOut.LatestStockOutUserId = user.Id;
        //可以出库了
        mastStockOut.StockStatus = ReagentStockStatusEnum.离库;
        mastStockOut.IsNoticed = false;
        //mastStockOut.Weight = weight;
        //记日志
        await _reagentStockHistoryRepository.InsertAsync(new ReagentStockHistory()
        {
            ReagentStockId = mastStockOut.Id,
            Content = $"试剂 被领用离库",
            CreateUserName = user.Name
        });

        await _reagentOperateRecordRepository.InsertAsync(new ReagentOperateRecord()
        {
            Weight = mastStockOut.Weight,
            BarCode = mastStockOut.BarCode,
            LocationId = mastStockOut.LocationId,
            LocationName = mastStockOut.LocationName,
            WarehouseId = mastStockOut.WarehouseId,
            Capacity = mastStockOut.Capacity,
            CapacityUnit = mastStockOut.CapacityUnit,
            SafeAttribute = mastStockOut.SafeAttribute,
            BatchNo = mastStockOut.BatchNo,
            OperateType = OperateTypeEnum.领用,
            ReagentId = mastStockOut.ReagentId,
            CreateUserName = user.Name,
            Year = DateTime.Now.Year,
            Month = DateTime.Now.Month,
            Day = DateTime.Now.Day,
            Hour = DateTime.Now.Hour,
            Minute = DateTime.Now.Minute
        });
    }

    /// <summary>
    /// 获取试剂信息
    /// </summary>
    /// <param name="barCode"></param>
    /// <returns></returns>
    public async Task<ReagentStockDto> GetReagentDetail(string barCode, int warehouseId)
    {
        if (barCode.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(barCode));
        }
        var entity = await _reagentStockRepository.GetAll()
            .Include(w=>w.Reagent)
            .FirstOrDefaultAsync(w => w.BarCode == barCode);
        var dto= this.ObjectMapper.Map<ReagentStockDto>(entity);
        if (dto == null)
        {
            //从普通试剂查找
            var entity2=await  _normalReagentStockRepository.GetAll()
                .Include(w => w.Reagent)
                .FirstOrDefaultAsync(w => w.BarCode == barCode && w.WarehouseId == warehouseId);
            if(entity2 == null)
            {
                throw new UserFriendlyException($"未找到条码 {barCode} 对应的库存信息");
            }
            return new ReagentStockDto()
            {
                Id = entity2.Id,
                BarCode = barCode,
                BatchNo = entity2.BatchNo,
                ReagentNo= entity2.Reagent.No,
                ReagentCnName= entity2.Reagent.CnName,
                ReagentEnName = entity2.Reagent.EnName,
                LocationName=entity2.LocationName,
                Capacity=entity2.Capacity,
                CapacityUnit=entity2.CapacityUnit,
                ReagentCasNo=entity2.CasNo,
                FirstStockInTime=entity2.StockInTime,
                LatestStockInTime=entity2.LatestStockOutTime,
                LatestStockOutUserName=entity2.LatestStockOutUserName,
                Amount=entity2.Amount,
                RealAmount=entity2.RealAmount,
                Price=entity2.Price, 
            };
        }
        else
        {
            dto.IsMaster = true;
        }
        return dto;
    }
    /// <summary>
    /// 专管试剂入库 
    /// </summary>
    /// <returns></returns>
    public async Task MasterStockIn(MasterStockInInputDto input)
    {
        var entity = await _reagentStockRepository.GetAsync(input.ReagentStockId);
        if (entity.StockStatus != ReagentStockStatusEnum.待入库 )
        {
            throw new UserFriendlyException("只有 待入库 的试剂才允许入库");
        }

        //是否审核通过
        if (entity.ClientConfirm && !entity.ClientConfirmed)
        {
            throw new UserFriendlyException("此试剂需要仓库管理员审核后才能入库");

        }
        if (entity.DoubleConfirm && !entity.DoubleConfirmed)
        {
            //双人双锁
            throw new UserFriendlyException("此试剂需要 至少两个专管试剂管理员审核后才能入库");
        }

        if (entity.WarehouseId != input.WarehouseId)
        {
            var wh = await _warehouseRepository.GetAsync(entity.WarehouseId);
            throw new UserFriendlyException($"此试剂不允许入库到仓库  请入库到 {wh.Name} 仓库内！");
        }


        //检查库位是否满足存放条件
        var location = _locationCache[entity.LocationId];
        if (location!=null && location.CountLimit > 0)
        {
            var exitCount=await _reagentStockRepository
                .CountAsync(w=>w.LocationId == entity.LocationId &&w.StockStatus== ReagentStockStatusEnum.在库);
            if (exitCount >= location.CountLimit)
            {
                throw new UserFriendlyException($"库位 {location.Name} 的限制库存量为：{location.CountLimit},现在已经存满，请联系管理员把此试剂更换库位存储，或者设置此库位的存储上限！");
            }
        }


        entity.Weight=input.Weight;

        var user = await GetCurrentUserAsync();
        entity.LatestStockInTime=DateTime.Now;;
        entity.LatestStockInUserName = user.Name;
        if (!entity.FirstStockInTime.HasValue)
        {
            //首次入库
            entity.FirstStockInTime=DateTime.Now; 
        }

        entity.StockStatus = ReagentStockStatusEnum.在库;
        await _reagentStockHistoryRepository.InsertAsync(new ReagentStockHistory()
        {
            ReagentStockId = entity.Id,
            Content = $"试剂 入库",
            CreateUserName = user.Name
        });

        await _reagentOperateRecordRepository.InsertAsync(new ReagentOperateRecord()
        {
            Weight = entity.Weight,
            BarCode = entity.BarCode,
            LocationId=entity.LocationId,
            LocationName = entity.LocationName,
            WarehouseId = entity.WarehouseId,
            Capacity=entity.Capacity,
            CapacityUnit = entity.CapacityUnit,
            SafeAttribute = entity.SafeAttribute,
            BatchNo=entity.BatchNo,
            OperateType=OperateTypeEnum.入库,
            ReagentId=entity.ReagentId,
            CreateUserName=user.Name,
            Year = DateTime.Now.Year,
            Month = DateTime.Now.Month,
            Day = DateTime.Now.Day,
            Hour = DateTime.Now.Hour,
            Minute = DateTime.Now.Minute
        });

    }

    /// <summary>
    /// 普通试剂入库
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async  Task NormalStockIn(MasterStockInInputDto input)
    {
        if (input.Acount <= 0)
        {
            throw new UserFriendlyException("入库数量不得小于0");
        }
        var entity = await _normalReagentStockRepository
            .GetAll()
            .Include(w => w.Reagent)
            .FirstOrDefaultAsync(w => w.Id == input.ReagentStockId);
        if (entity == null)
        {
            throw new UserFriendlyException("未找到试剂信息：" + input.ReagentStockId.ToString());
        }
         
        var wh = await _warehouseRepository.GetAsync(entity.WarehouseId);
        if (entity.WarehouseId != input.WarehouseId)
        {
           
            throw new UserFriendlyException($"此试剂不允许入库到仓库  请入库到 {wh.Name} 仓库内！");
        }

        //检查库位是否满足存放条件
        var location = _locationCache[entity.LocationId];
        if (location != null && location.CountLimit > 0)
        {
            var exitCount = _normalReagentStockRepository.GetAll()
                .Where(w => w.LocationId == entity.LocationId).DefaultIfEmpty().Sum(w=>w.RealAmount);
            if (exitCount >= location.CountLimit)
            {
                throw new UserFriendlyException($"库位 {location.Name} 的限制库存量为：{location.CountLimit},现在已经存满，请联系管理员把此试剂更换库位存储，或者设置此库位的存储上限！");
            }
        }


        var user = await GetCurrentUserAsync();
        if(!entity.StockInTime.HasValue)
            entity.StockInTime = DateTime.Now; ;
        entity.LatestStockInUserName = user.Name;
        entity.LatestStockInTime = DateTime.Now;
        entity.RealAmount += input.Acount;
        //if (!entity.FirstStockInTime.HasValue)
        //{
        //    //首次入库
        //    entity.FirstStockInTime = DateTime.Now;
        //}

       // entity.StockStatus = ReagentStockStatusEnum.在库;
        //await _reagentStockHistoryRepository.InsertAsync(new ReagentStockHistory()
        //{
        //    ReagentStockId = entity.Id,
        //    Content = $"试剂 入库",
        //    CreateUserName = user.Name
        //});

        await _normalReagentOperateRecordRepository.InsertAsync(new NormalReagentOperateRecord()
        {
            NormalReagentStockId=entity.Id,
            BarCode = entity.BarCode,
            LocationId = entity.LocationId,
            LocationName = entity.LocationName,
            WarehouseId = entity.WarehouseId, 
            WarehouseName= wh.Name,
            Capacity = entity.Capacity,
            CapacityUnit = entity.CapacityUnit,
            OperateAmount=input.Acount,
            //SafeAttribute = entity.SafeAttribute,
            BatchNo = entity.BatchNo,
            OperateType = OperateTypeEnum.入库,
            ReagentId = entity.ReagentId,
            ReagentNo=entity.Reagent.No,
            ReagentCasNo=entity.Reagent.CasNo,
            ReagentCnName=entity.Reagent.CnName,
            ReagentEnName=entity.Reagent.EnName,
            ReagentReagentCatalog =entity.Reagent.ReagentCatalog,
            ReagentPinYinCode=entity.Reagent.PinYinCode,
            CreateUserName = user.Name,
            Year = DateTime.Now.Year,
            Month = DateTime.Now.Month,
            Day = DateTime.Now.Day,
            Hour = DateTime.Now.Hour,
            Minute = DateTime.Now.Minute
        });

    }

    /// <summary>
    /// 获取专管试剂归还详情
    /// </summary>
    /// <param name="barCode"></param>
    /// <returns></returns>
    public async Task<ReagentStockDto> GetMasterStockBackDetail(string barCode)
    {
        if (barCode.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(barCode));
        }
        var entity = await _reagentStockRepository.GetAll().Include(w => w.Reagent)
            .FirstOrDefaultAsync(w => w.BarCode == barCode);
        var dto = this.ObjectMapper.Map<ReagentStockDto>(entity);
        if (dto == null)
        {
            //从普通试剂查找
            throw new UserFriendlyException($"未找到条码为{barCode}的试剂");
        }
        else if(dto.StockStatus!=ReagentStockStatusEnum.离库)
        {
            throw new UserFriendlyException($"只有离库的试剂才能归还！");
        }
        dto.IsMaster = true;
        return dto;
    }

    /// <summary>
    /// 专管试剂 归还
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task MasterStockBack(MasterStockBackInputDto input)
    {
        foreach (var item in input.ReagentStockIds)
        {
            await TaskSingleMasterStockBack(item, input.WarehouseId);
        }
    }

    /// <summary>
    /// 专管试剂 归还 V2
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task MasterStockBackV2(MasterStockBackV2InputDto input)
    {
        await TaskSingleMasterStockBack(input.ReagentStockId, input.WarehouseId,input.Weight);
    }

    private async Task TaskSingleMasterStockBack(int reagentStockId,int warehouseId,decimal weight=0)
    {
        var entity = await _reagentStockRepository.GetAsync(reagentStockId);
        if (entity.StockStatus != ReagentStockStatusEnum.离库)
        {
            throw new UserFriendlyException("只有 离库 的试剂才允许归还");
        }

        if (entity.WarehouseId != warehouseId)
        {
            var wh = await _warehouseRepository.GetAsync(entity.WarehouseId);
            throw new UserFriendlyException($"此试剂不允许入库到仓库  请入库到 {wh.Name} 仓库内！");
        }

        var user = await GetCurrentUserAsync();
        entity.LatestStockInTime = DateTime.Now; ;
        entity.LatestStockInUserName = user.Name;
        if (!entity.FirstStockInTime.HasValue)
        {
            //首次入库
            entity.FirstStockInTime = DateTime.Now;
        }

        entity.StockStatus = ReagentStockStatusEnum.在库;
        entity.Weight = weight;
        await _reagentStockHistoryRepository.InsertAsync(new ReagentStockHistory()
        {
            ReagentStockId = entity.Id,
            Content = $"试剂  归还",
            CreateUserName = user.Name
        });

        await _reagentOperateRecordRepository.InsertAsync(new ReagentOperateRecord()
        {
            Weight = weight,
            BarCode = entity.BarCode,
            LocationId = entity.LocationId,
            LocationName = entity.LocationName,
            WarehouseId = entity.WarehouseId,
            Capacity = entity.Capacity,
            CapacityUnit = entity.CapacityUnit,
            SafeAttribute = entity.SafeAttribute,
            BatchNo = entity.BatchNo,
            OperateType = OperateTypeEnum.归还,
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
    /// 获取我的生效的订单
    /// </summary>
    /// <param name="warehouseId"></param>
    /// <returns></returns>
    public async Task<List<OutOrderDto>> GetMyOrder(int warehouseId) {
        var uid = AbpSession.GetUserId();
          var query=await _outOrderRepository.GetAll()
            //.Include(w=>w.ApplyUser)
            .Include(w=>w.OutOrderMasterItems).ThenInclude(w=>w.ReagentStock)
            .Where(w=> (w.CreatorUserId== uid || w.ApplyUserId==uid) 
                    && w.OutOrderStatus==OutOrderStatusEnum.待出库
                    && w.WarehouseId==warehouseId) .ToListAsync();

        return ObjectMapper.Map<List<OutOrderDto>>(query);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="barCode"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task<NormalReagentStockListDto> GetNormalByCode(string barCode)
    {
        if (barCode.IsNullOrEmpty()) { throw new UserFriendlyException("请输入条码！"); }
        barCode= barCode.Trim();
        var query=await _normalReagentStockRepository.FirstOrDefaultAsync(w=>w.BarCode==barCode);
        if(query==null) { throw new UserFriendlyException($"未找到条码{barCode}对应的试剂"); }

        return ObjectMapper.Map<NormalReagentStockListDto>(query);
    }
        

    /// <summary>
    /// 通过订单ID取货
    /// </summary>
    /// <returns></returns>
    public async Task<OutOrderDto> GetOrderByOrderId(int orderId) {
        
        var order= await _outOrderRepository.GetAll().Include(w => w.OutOrderMasterItems).ThenInclude(w => w.ReagentStock)
            .FirstOrDefaultAsync(w=>w.Id==orderId );
        if (order == null)
        {
            throw new UserFriendlyException("订单不存在");
        }
       
        if (order.OutOrderStatus != OutOrderStatusEnum.待出库)
        {
            throw new UserFriendlyException("只有待出库的订单可以出库");
        }
        if (order.OutOrderType == OutOrderTypeEnum.专管试剂 )
        {
            if (order.CreatorUserId != AbpSession.UserId)
            { 
                throw new UserFriendlyException("专管出库单只能申请者本人来取");
            }
        }
        return ObjectMapper.Map<OutOrderDto>(order);
    }

    /// <summary>
    /// 订单出库
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task OutOrderStockOut(OutOrderStockOutInputDto input)
    {
        var uid=AbpSession.GetUserId();
        var order = await _outOrderRepository
            .GetAll()
            .Include(w => w.OutOrderMasterItems)
            .FirstOrDefaultAsync(w => w.Id == input.OrderId);
        if (order == null)
        {
            throw new UserFriendlyException("订单不存在");
        }
        if(order.WarehouseId != input.WarehouseId)
        {
            throw new UserFriendlyException($"此出库单不属于本仓库，无法出库。请到仓库{order.WarehouseName}出库");
        }

        if (order.OutOrderStatus != OutOrderStatusEnum.待出库)
        {
            throw new UserFriendlyException("只有待出库的钉钉可以出库");
        }
        if (order.OutOrderType == OutOrderTypeEnum.专管试剂)
        {
            if (order.CreatorUserId != uid)
            {
                throw new UserFriendlyException("专管出库单只能申请者本人来取");
            } 

            //判断订单项目是否都审核通过了
            foreach (var item in order.OutOrderMasterItems)
            {
                if (item.ClientConfirm)
                {
                    if(item.ClientConfirmed == OutOrderMasterItemStatues.待审核)
                    {
                        throw new UserFriendlyException("此订单需要 终端仓库管理员 审核后才能出库");
                    }
                }
                if (item.DoubleConfirm)
                {
                    if (item.DoubleConfirmed == OutOrderMasterItemStatues.待审核)
                    {
                        throw new UserFriendlyException("此订单需要 双人双锁 审核后才能出库");
                    }
                }
            }
        }
        else
        {
            //说明是普通试剂
            if(input.Items.Count == 0)
            {
                throw new UserFriendlyException("此订单是普通出库单，请扫描试剂瓶身的条码，并且输入数量后再出库！");
            }
        }
        //说明审核都通过了，但是也有可能其中一项审核不通过
        switch (order.OutOrderType)
        {
            case OutOrderTypeEnum.专管试剂:
                await MasterOutOrderOut(order);
                break;
            case OutOrderTypeEnum.普通试剂:
                await CommonOutOrderOut(order,input);
                break; 
        }
    }

    /// <summary>
    /// 专管出库
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    private async Task MasterOutOrderOut(OutOrder order)
    {
        foreach (var item in order.OutOrderMasterItems)
        {
            if (item.ClientConfirm)
            {
                if (item.ClientConfirmed != OutOrderMasterItemStatues.审核通过)
                {
                    break;
                }
            }
            if (item.DoubleConfirm)
            {
                if (item.DoubleConfirmed != OutOrderMasterItemStatues.审核通过)
                {
                    break;
                }
            }
            var entity = await _reagentStockRepository.GetAsync(item.ReagentStockId.Value);
            entity.LockedOrderId = null; //清除锁定状态
            await MasterOut(entity, false );
            item.StockOutTime = DateTime.Now;
        }
        order.OutOrderStatus = OutOrderStatusEnum.出库完毕;
    }
    /// <summary>
    /// 普通出库
    /// </summary>
    /// <param name="order"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task CommonOutOrderOut(OutOrder order,OutOrderStockOutInputDto input)
    {
        var currentUserName = (await GetCurrentUserAsync()).Name;
        //可以出库咯
        foreach (var item in order.OutOrderMasterItems)
        {
            int totalItemAccount = item.StockoutAccount;
            //1.0 先把 这个库位的所有批次招到
            var normoals =  _normalReagentStockRepository
                .GetAll()
                .Include(w=>w.Reagent)
                .Where(w => w.WarehouseId == order.WarehouseId && w.LocationId == item.LocationId);
            //可能在一个库位里会有多个试剂
            //判断数量
            // 2.0 在这些批次里招到用户出的数量
          
            for (int i = 0; i < input.Items.Count; i++)
            {
                var iitem =input.Items[i];
                if (totalItemAccount <= 0) break;
                var singleNormal = normoals.FirstOrDefault(w => w.BarCode == iitem.BarCode);
                if (singleNormal != null)
                {
                    //尽量卡一下 当前数量必须要小于出库数量
                    if (iitem.Account > totalItemAccount)
                    {
                        //出库数量不应该大
                        break;
                    }

                    //说明找到了一波数据
                    await CommonOut(
                        singleNormal,
                        iitem.Account,
                        currentUserName + ","+ order.ApplyUserName);
                    item.Price = singleNormal.Price;//正在出库的时候才知道具体的价格
                    totalItemAccount -= iitem.Account;
                    //把这一项从集合中移除
                    //input.Items.RemoveAt(i);
                }
                else
                { //AIEOU2KYVX
                }
            }
      
            if (totalItemAccount != 0)
            {
                //说明出的不对
                throw new UserFriendlyException("出库失败",$"订单项 {item.ReagentCnName} 出库数量和实际出库数量不吻合，无法出库！" +
                    $"请检查试剂 {item.ReagentCasNo??item.ReagentCnName} 是否从位置 {item.LocationName} 取出！");
            }
            //释放被锁订单
            await _outOrderDomainService.ReleaseCommonStockItem(item);
            item.StockOutTime = DateTime.Now;
        }
        order.OutOrderStatus = OutOrderStatusEnum.出库完毕;
    }
}