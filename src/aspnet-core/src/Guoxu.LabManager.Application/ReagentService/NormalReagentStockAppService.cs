using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.Caching;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.ReagentService.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Guoxu.LabManager.Dto;
using Guoxu.LabManager.Commons.Dto;

namespace Guoxu.LabManager.ReagentService
{
    [AbpAuthorize()]
    public class NormalReagentStockAppService : LabManagerAppServiceBase
    {
        private readonly IRepository<NormalReagentStock> _normalReagentStockRepository;
        private readonly IRepository<Reagent> _reagentRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<ReagentLocation> _reagentLocationRepository;
        private readonly CompanyDomainService _companyDomainService; 
        private readonly IRepository<NormalReagentOperateRecord, long> _normalReagentOperateRecordRepository;
        private readonly IDapperRepository<NormalReagentStock> dapperRepository;
        private readonly ILocationCache _locationCache;
        public NormalReagentStockAppService(
            IRepository<NormalReagentStock> normalReagentStockRepository,
            IRepository<Reagent> reagentRepository,
            IRepository<Location> locationRepository,
            IRepository<ReagentLocation> reagentLocationRepository,
            CompanyDomainService companyDomainService,
            IDapperRepository<NormalReagentStock> dapperRepository,
            IRepository<NormalReagentOperateRecord, long> normalReagentOperateRecordRepository, 
            ILocationCache locationCache)
        {
            _normalReagentStockRepository = normalReagentStockRepository;
            _reagentRepository = reagentRepository;
            _locationRepository = locationRepository;
            _reagentLocationRepository = reagentLocationRepository;
            _companyDomainService = companyDomainService;
            this.dapperRepository = dapperRepository;
            _normalReagentOperateRecordRepository = normalReagentOperateRecordRepository;
            _locationCache = locationCache;
        }

        private string[] randomList = new[]
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z","0","1","2"
            ,"3","4","5","6","7","8"
        };

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns> 
        [AbpAuthorize(PermissionNames.Pages_Reagent_Ruku)]
        public async Task<CreateNormalReagentStockDto> CreateNormalStock(CreateNormalReagentStockDto input)
        {
            var user = await GetCurrentUserAsync(); 
            var reagent = await _reagentRepository.GetAsync(input.ReagentId);
            if (reagent.ReagentCatalog == ReagentCatalog.专管试剂)
            {
                throw new UserFriendlyException("专管试剂不允许在普通试剂入库界面入库！");
            }

            if (input.ProductionDate.HasValue)
            {
                //如果设置了生产日期，  就以生产日期加保质期 为过期日期！
                input.ExpirationDate = input.ProductionDate.Value.Date.AddMonths(input.ExpirationMonth);
            }

            var cItem = input.Items.Where(w => w.Amount > 0);

            foreach (var createNormalReagentStockItemDto in cItem)
            {
                var reagentLocation = await _reagentLocationRepository.FirstOrDefaultAsync(
                w => w.LocationId == createNormalReagentStockItemDto.LocationId 
                && w.ReagentId == input.ReagentId);
                if (reagentLocation == null)
                {
                    throw new UserFriendlyException($"选中的位置不能存储试剂 {reagent.CnName}！");
                }

                //判断批次是否已经有了，如果有批次，则直接加库存，否则再新增
                var exitStock = await _normalReagentStockRepository
                    .FirstOrDefaultAsync(w => w.ReagentId == input.ReagentId
                    && w.BatchNo == input.BatchNo
                    && w.LocationId==createNormalReagentStockItemDto.LocationId
                    );
                if (exitStock != null)
                {
                    exitStock.Amount += createNormalReagentStockItemDto.Amount;
                    await _normalReagentStockRepository.UpdateAsync(exitStock);
                    createNormalReagentStockItemDto.BarCode = exitStock.BarCode;
                    continue;

                }
                 
                var location = await _locationRepository.GetAsync(createNormalReagentStockItemDto.LocationId);
                var entity = this.ObjectMapper.Map<NormalReagentStock>(input);
                entity.Amount = createNormalReagentStockItemDto.Amount;
                entity.LocationId = createNormalReagentStockItemDto.LocationId;
                entity.LocationName = location.Name;
                entity.CreateUserName = user.Name;
                entity.WarehouseId = location.WarehouseId;
                entity.Capacity = reagent.Capacity;
                entity.CapacityUnit = reagent.CapacityUnit;
                entity.PinYinCode = reagent.PinYinCode;
                entity.CasNo = reagent.CasNo;
                entity.ProductionDate = input.ProductionDate;
                entity.ExpirationDate = input.ExpirationDate.Value;
                entity.ExpirationMonth = input.ExpirationMonth;
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

                entity.BarCode = string.Join("", RandomHelper.GenerateRandomizedList(randomList).Take(10));

                await _normalReagentStockRepository.InsertAndGetIdAsync(entity);
                createNormalReagentStockItemDto.BarCode = entity.BarCode; 
            }

            return input;
        }

        public async Task<LocationStockDto> GetLocationStock(int locationId)
        {
            var result = new LocationStockDto();
            var location = _locationCache[locationId];
            result.CountLimit = location.CountLimit;
            result.ExitStockCount =_normalReagentStockRepository
                .GetAll()
                .Where(w => w.LocationId == locationId )
                .Sum(q=>q.RealAmount);  
            return result;
        }

        public async Task Delete(EntityDto input)
        {
            var entity = await this._normalReagentStockRepository.GetAsync(input.Id);
            if (entity.RealAmount!=0)
            {
                throw new UserFriendlyException("这批次普通试剂已经有入库操作，无法删除");
            }
            await this._normalReagentStockRepository.DeleteAsync(entity);
        }

        private void AddCreateNormalReagentOperateRecord()
        {
           
        }

        [AbpAuthorize(PermissionNames.Pages_Reagent_Ruku)]
        public async Task<PagedResultDto<NormalReagentStockListDto>> GetAllNormal(GetAllMasterInputDto input)
        {
            var query = _normalReagentStockRepository.GetAll()
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
                .Include(w => w.Warehouse)
                .Include(w => w.Reagent)
                .OrderBy(input.Sorting)
                .PageBy(input);
            var dto = await this.ObjectMapper.ProjectTo<NormalReagentStockListDto>(list)
                .ToListAsync();
            return new PagedResultDto<NormalReagentStockListDto>(count, dto);
        }

        /// <summary>
        /// 操作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Reagent_CaozuoJilu)]
        public async Task<PagedResultDto<ReagentOperateRecordDto>> GetNormalReagentOperateRecord(ReagentOperateRecordDtoInputDto input)
        {

                var query =   _normalReagentOperateRecordRepository.GetAll()
                  .WhereIf(!input.ReagentNo.IsNullOrEmpty(), w => w.ReagentNo.Contains(input.ReagentNo))
                  .WhereIf(!input.Filter.IsNullOrEmpty(),
                     w => w.ReagentCnName.Contains(input.Filter) || w.ReagentEnName.Contains(input.Filter))
                 .WhereIf(!input.BarCode.IsNullOrEmpty(), w => EF.Functions.Like(w.BarCode, $"%{input.BarCode}%"))
                 .WhereIf(!input.CreateUserName.IsNullOrEmpty(),
                    w => EF.Functions.Like(w.CreateUserName, $"%{input.CreateUserName}%"))
                .WhereIf(input.StartDate.HasValue, w => w.CreationTime >= input.StartDate)
                .WhereIf(input.EndDate.HasValue, w => w.CreationTime <= input.EndDate)
                .WhereIf(input.OperateType.HasValue, w => w.OperateType == input.OperateType)
                .WhereIf(!input.CasNo.IsNullOrEmpty(), w => w.ReagentCasNo.Contains(input.CasNo))
                .WhereIf(input.WarehouseId.HasValue, w => w.WarehouseId == input.WarehouseId);
                var count = await query.CountAsync();
                var list = await query
                    //.Include(w => w.Reagent)
                    //.Include(w => w.Warehouse)
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();
                var dto = this.ObjectMapper.Map<List<ReagentOperateRecordDto>>(list);
                return new PagedResultDto<ReagentOperateRecordDto>(count, dto); 
        }


        [AbpAuthorize(PermissionNames.Pages_Reagent_CaozuoJilu, PermissionNames.Pages_Reagent_Ruku)]
        public async Task<PagedResultDto<ReagentOperateRecordDto>> GetOperateRecordByNormalReagentId(PagedResultRequestFilterDto input)
        {
            var normalReagentStockId = Convert.ToInt32(input.Filter);

            var query = _normalReagentOperateRecordRepository.GetAll()
                .Where(w=>w.NormalReagentStockId== normalReagentStockId);
            var count = await query.CountAsync();
            var list = await query 
                .OrderByDescending(w=>w.Id)
                .PageBy(input)
                .ToListAsync();
            var dto = this.ObjectMapper.Map<List<ReagentOperateRecordDto>>(list);
            return new PagedResultDto<ReagentOperateRecordDto>(count, dto);
        }

        /// <summary>
        /// 普通库存查询
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Reagent_KucunChaxun)]
        public async Task<PagedResultDto<ClientStockDto>> GetNormalReagentStock(GetMasterReagentStockInputDto input)
        {
            input.MaxResultCount++;

            string sql = $@"select a.Num,a.minPrice,a.maxPrice, {(input.GroupByLocationId ? "a.LocationId," : "")} w.name as WarehouseName,b.* from
(select ReagentId,WarehouseId,min(price) as minPrice,max(price) as maxPrice,sum(RealAmount) as Num {(input.GroupByLocationId ? ",LocationId" : "")} from NormalReagentStock  where isdeleted=0
{(input.WarehouseId.HasValue ? " and WarehouseId=@WarehouseId" : "")} 
{(input.StockStatus.HasValue && input.StockStatus.Value < 3 ? GetSql(input.StockStatus.Value) : "")}
group by ReagentId,WarehouseId {(input.GroupByLocationId? ",LocationId":"")} {(input.StockShouldMoreZero? " having sum(RealAmount)>0" : "")}
) a
inner join [dbo].[Reagent]  b
on a.ReagentId =b.Id
{(input.No.IsNullOrEmpty() ? "" : " and b.No=@No")}
{(input.CasNo.IsNullOrEmpty() ? "" : " and b.CasNo=@CasNo")}
{(input.Filter.IsNullOrEmpty() ? "" : " and b.CnName like '%'+@Filter+'%'")}
{(input.Purity.IsNullOrEmpty() ? "" : " and b.Purity=@Purity")}
{(input.StorageCondition.IsNullOrEmpty() ? "" : " and b.StorageCondition=@StorageCondition")}
{(input.StockStatus.HasValue && input.StockStatus.Value == 3 ? " and (b.InventoryWarning>0 and b.InventoryWarning>=a.Num)" : "")}
inner join Warehouse w on a.WarehouseId=w.Id
order by ReagentId
offset @SkipCount rows fetch next @MaxResultCount rows only ";
            var query = await dapperRepository.QueryAsync<ClientStockDto>(sql, input);
            if (input.GroupByLocationId)
            {
                foreach (var item in query)
                {
                    item.LocationName = _locationCache[ item.LocationId.Value].Name;
                }

            } 
            return new PagedResultDto<ClientStockDto>(
                input.SkipCount + query.Count(),
                query.Take(input.MaxResultCount--)
                .ToList());
        }

        /// <summary>
        /// 1 濒临过期
        /// 2 已过期
        /// 3 库存紧张
        /// </summary>
        string GetSql(int stockStatus)
        {
            if (stockStatus == 1)
            {
                return " and DATEDIFF(day,GETDATE(),ExpirationDate) <30 and  DATEDIFF(day,GETDATE(),ExpirationDate)>0";
            }
            else
            {
                return " and DATEDIFF(day,GETDATE(),ExpirationDate) <0";
            }
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
  sum(OperateAmount) as num
   from NormalReagentOperateRecord 
where CreationTime >@StartDate 
{(input.ReagentId.HasValue? " and ReagentId=@ReagentId":"")}
and CreationTime< @EndDate 
   group by
  {(GetGroupBy(input.GroupBy))}, 
OperateType";
            var result = new GerChartDataOutDto();
            var query = await dapperRepository.QueryAsync<TempGerChartDataClass>(sql, input);//.ToListAsync();

            result.Time = query.GroupBy(w => w.Time)
                .Select(w => w.Key)
                .OrderBy(w => w).ToList();
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
        /// 通过试剂编号获取试剂列表
        /// </summary>  
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<NormalReagentStockListDto>> GetNormalReagentStockDetailByNo(GetMasterReagentStockDetailByNoInput input)
        {
            var query = this._normalReagentStockRepository
                .GetAll()
                .Where(w =>  w.Reagent.No == input.RegentNo && w.RealAmount>0);
    
            var count = await query.CountAsync();
            var list = await query
                .Include(w => w.Warehouse)
                .Include(w => w.Reagent)
                .OrderBy(w => w.ExpirationDate)
                .PageBy(input)
                .ToListAsync();
            return new PagedResultDto<NormalReagentStockListDto>(count, ObjectMapper.Map<List<NormalReagentStockListDto>>(list));
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
}
