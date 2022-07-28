
using Abp.Domain.Uow;
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.BaseInfo.Dto;
using Guoxu.LabManager.Caching;
using Guoxu.LabManager.Commons;
using Guoxu.LabManager.Commons.Dto;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;
using Guoxu.LabManager.Storage;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;

namespace Guoxu.LabManager.BaseInfo;

[AbpAuthorize()]
public class ReagentAppService : LabManagerAppServiceBase
{
    private readonly CompanyDomainService _companyDomainService;
    private readonly IRepository<Reagent> _reagentRepository;
    private readonly IRepository<ReagentLocation> _reagentLocationRepository;
    private readonly IRepository<ReagentStock> _reagentStockRepository;
    private readonly IRepository<NormalReagentStock> _normalReagentStockRepository;
    private readonly ITempFileCacheManager _tempFileCacheManager;

    public ReagentAppService(
        IRepository<Reagent> reagentRepository,
        CompanyDomainService companyDomainService,
        IRepository<ReagentLocation> reagentLocationRepository,
        IRepository<ReagentStock> reagentStockRepository,
        IRepository<NormalReagentStock> normalReagentStockRepository, 
        ITempFileCacheManager tempFileCacheManager)
    {
        _reagentRepository = reagentRepository;
        _companyDomainService = companyDomainService;
        _reagentLocationRepository = reagentLocationRepository;
        _reagentStockRepository = reagentStockRepository;
        _normalReagentStockRepository = normalReagentStockRepository;
        _tempFileCacheManager = tempFileCacheManager;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AbpAuthorize(PermissionNames.Pages_BaseInfo_Reagent)]
    public async Task Create(ReagentDto input)
    {
        var entity = this.ObjectMapper.Map<Reagent>(input);
        entity.ReagentCatalog = input.ReagentCatalog;
        if (entity.ProductionCompanyId.HasValue)
        {
            entity.ProductionCompanyName = _companyDomainService.GetAllCompany(true)
                .FirstOrDefault(w => w.Id == entity.ProductionCompanyId)?.Name;
        }
        if (entity.SupplierCompanyId.HasValue)
        {
            entity.SupplierCompanyName = _companyDomainService.GetAllCompany(true)
                .FirstOrDefault(w => w.Id == entity.SupplierCompanyId)?.Name;
        }

        //限制一下， 如果时专管的话，只能选择一个库位
        if(input.ReagentCatalog==ReagentCatalog.专管试剂)
        {

            if (input.ReagentLocationIds.Length != 1)
            {
                throw new UserFriendlyException("专管只是必须并且只能选择一个存放库位！");
            }

            //判断存储属性是否相符
        }

        if (input.ReagentLocationIds.Any())
        {
            foreach (var inputReagentLocation in input.ReagentLocationIds)
            {
                entity.ReagentLocations.Add(new ReagentLocation()
                {
                    LocationId = inputReagentLocation
                });
            }
        }
        try
        {
            await _reagentRepository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {

            throw new UserFriendlyException(ex.InnerException?.Message);
        }
        catch(Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AbpAuthorize(PermissionNames.Pages_BaseInfo_Reagent)]
    public async Task Delete(EntityDto input)
    {
        //判断是否有库存
        var w = await _reagentStockRepository.GetAll() .AnyAsync(q => q.ReagentId == input.Id 
        && q.StockStatus==ReagentStockStatusEnum.在库);
        if (w)
        {
            throw new Abp.UI.UserFriendlyException("此试剂还存在在库库存(专管)，无法删除，请把所有在库库存出库后再删除！");
        }

        var w2 = await _normalReagentStockRepository.GetAll().AnyAsync(w => w.ReagentId == input.Id && w.RealAmount > 0);
        if (w2)
        {
            throw new Abp.UI.UserFriendlyException("此试剂还存在在库库存(普通)，无法删除，请把所有在库库存出库后再删除！");
        }

        var entity = await _reagentRepository.GetAsync(input.Id);
        entity.No = entity.No + "_Deleted_"+DateTime.Now.ToString("yyyydd");
        await this._reagentRepository.UpdateAsync(entity);
        await CurrentUnitOfWork.SaveChangesAsync();
        await this._reagentRepository.DeleteAsync(entity);
    }

    [AbpAuthorize(PermissionNames.Pages_BaseInfo_Reagent)]
    public async Task Update(ReagentDto input)
    {
        var entity = await this._reagentRepository
            .GetAll()
            .Include(w => w.ReagentLocations)
            .FirstOrDefaultAsync(w => w.Id == input.Id);
        //判断是否修改供应商
        if (input.ProductionCompanyId != entity.ProductionCompanyId)
        {
            if (input.ProductionCompanyId.HasValue)
            {
                input.ProductionCompanyName = _companyDomainService.GetAllCompany(true)
                    .FirstOrDefault(w => w.Id == input.ProductionCompanyId)?.Name;
            }
            else
            {
                input.ProductionCompanyName = null;
            }
        }

        if (input.SupplierCompanyId != entity.SupplierCompanyId)
        {
            if (input.SupplierCompanyId.HasValue)
            {
                input.SupplierCompanyName = _companyDomainService.GetAllCompany(true)
                    .FirstOrDefault(w => w.Id == input.SupplierCompanyId)?.Name;
            }
            else
            {
                input.SupplierCompanyName = null;
            }
        }


        //检测专管试剂存放位置
        //限制一下， 如果时专管的话，只能选择一个库位
        if (entity.ReagentCatalog == ReagentCatalog.专管试剂)
        { 
            if (input.ReagentLocationIds.Length != 1)
            {
                throw new UserFriendlyException("专管只是必须并且只能选择一个存放库位！");
            }
            if(entity.ReagentLocations.Any() 
                && 
                input.ReagentLocationIds[0] != entity.ReagentLocations.ToList()[0].LocationId)
            {
                // 说明换库位了  验证库里是否还有此试剂，如果还有 则不允许换
                 var exitStock=await _reagentStockRepository
                    .GetAll()
                    .FirstOrDefaultAsync(w=>w.ReagentId==entity.Id && w.StockStatus <= ReagentStockStatusEnum.在库);
                if (exitStock!=null)
                {
                    throw new UserFriendlyException($"此试剂已存在库位 {exitStock.LocationName} 不允许更改库位，除非把此试剂从库位 {exitStock.LocationName} 全部出库后才允许更改存储位置！");
                }
            }
            //判断存储属性是否相符
        }

        ObjectMapper.Map(input, entity);

        if (input.ReagentLocationIds != null)
        {
            foreach (var entityReagentLocation in entity.ReagentLocations)
            {
                if (input.ReagentLocationIds.All(w => w != entityReagentLocation.LocationId))
                {
                    await _reagentLocationRepository.DeleteAsync(entityReagentLocation.Id);
                }
            }

            foreach (var inputReagentLocationId in input.ReagentLocationIds)
            {
                if (entity.ReagentLocations.All(ur => ur.LocationId != inputReagentLocationId))
                {
                    await _reagentLocationRepository.InsertAsync(new ReagentLocation()
                    {
                        ReagentId = input.Id,
                        LocationId = inputReagentLocationId
                    });
                }
            }
        }

        try
        {
            await _reagentRepository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {

            throw new UserFriendlyException(ex.InnerException?.Message);
        }
        catch (Exception ex)
        {
            throw;
        }

       

    }

    [HttpPost]
    public async Task<PagedResultDto<ReagentDto>> GetAll(GetReagentDto input)
    {
        var query = this._reagentRepository.GetAll()
            .WhereIf(!input.No.IsNullOrEmpty(), w => w.No.Contains(input.No))
            .WhereIf(!input.CasNo.IsNullOrEmpty(), w => w.CasNo.Contains(input.CasNo))
            .WhereIf(!input.Filter.IsNullOrEmpty(), w => w.CnName.Contains(input.Filter) || w.CnAliasName.Contains(input.Filter) || w.EnName.Contains(input.Filter))
            .WhereIf(!input.ProductionCompanyName.IsNullOrEmpty(), w => w.ProductionCompanyName.Contains(input.ProductionCompanyName))
            .WhereIf(!input.SupplierCompanyName.IsNullOrEmpty(), w => w.SupplierCompanyName.Contains(input.SupplierCompanyName))
            .WhereIf(!input.ReagentCatalogs.IsNullOrEmpty(), w =>input.ReagentCatalogs.Contains(w.ReagentCatalog))
            .WhereIf(input.ReagentStatus.HasValue, w => w.ReagentStatus == input.ReagentStatus);

        var count = await query.CountAsync();
        var list = await query
            //.Include(w=>w.ReagentLocations)
            .OrderBy(input.Sorting)
            .PageBy(input)
            .ToListAsync();
        var dto = this.ObjectMapper.Map<List<ReagentDto>>(list);
      
        return new PagedResultDto<ReagentDto>(count,dto);
    }

    /// <summary>
    /// 查询已删除的试剂
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedResultDto<ReagentDto>> GetAllDeleted(PagedResultRequestDto input)
    {
        using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
        {
            var query = this._reagentRepository.GetAll().Where(w=>w.IsDeleted);

            var count = await query.CountAsync();
            var list = await query
                .Include(w=>w.ReagentLocations)
                .OrderByDescending(w=>w.Id)
                .PageBy(input)
                .ToListAsync(); 
            return new PagedResultDto<ReagentDto>(count, this.ObjectMapper.Map<List<ReagentDto>>(list));
        }
    }

    public async Task<ReagentDto> GetForEdit(NullableIdDto input)
    {
        if (!input.Id.HasValue)
        {
            return new ReagentDto();
        }
        var entity = await this._reagentRepository.GetAll()
            .Include(w => w.ReagentLocations)
            .ThenInclude(w=>w.Location)
            .FirstOrDefaultAsync(w => w.Id == input.Id);
        return ObjectMapper.Map<ReagentDto>(entity);
    }


    public async Task<string> ExcelImport(string fileToken)
    {
        var steam =  _tempFileCacheManager.GetFile(fileToken);
        if(steam == null)
        {
            throw new UserFriendlyException("文件已经过期！");
        }
        using (var stream = new MemoryStream(steam.Bytes))
        {
            var columns= MiniExcel.GetColumns(stream,startCell:"A2",useHeaderRow:true).ToList();
            if (columns.Count < 18)
            {
                throw new UserFriendlyException("请勿删除模板中的列信息，模板应该为19列数据！");
            }
            //验证
            if (columns[0].Trim() != "编号"
                || columns[1].Trim() != "状态"
                || columns[2].Trim() != "试剂类型"
                || columns[3].Trim() != "CAS号"
                || columns[4].Trim() != "中文名"
                || columns[5].Trim() != "拼音码")
            {
                throw new UserFriendlyException("请勿修改模板前两行数据，模板前6列应该为：编码，状态，试剂类型，CAS号，中文名，拼音码");
            }

            var rows = stream.Query<ReagentExcelDto>(startCell:"A2",sheetName: "试剂信息导入").ToList();  
            if(!rows.Any())
            {
                throw new UserFriendlyException("请至少输入一行试剂信息！");
            }
            rows=rows.Where(w=>!w.No.IsNullOrEmpty()).ToList();
            //数据完整性检查
            #region 数据完整性检查
            //if (rows.Any(q => q.No.IsNullOrEmpty()))
            //{
            //    throw new UserFriendlyException("导入失败！", "导入的数据中存在编号为空的行！");
            //}
            if (rows.Any(q => q.ReagentStatus.IsNullOrEmpty()))
            {
                throw new UserFriendlyException("导入失败！", "导入的数据中存在试剂状态为空的行！");
            }
            if (rows.Any(q => q.ReagentCatalog.IsNullOrEmpty()))
            {
                throw new UserFriendlyException("导入失败！", "导入的数据中存在试剂类型为空的行！");
            }
            if (rows.Any(q => q.CnName.IsNullOrEmpty()))
            {
                throw new UserFriendlyException("导入失败！", "导入的数据中存在中文名为空的行！");
            }
            if (rows.Any(q => q.PinYinCode.IsNullOrEmpty()))
            {
                throw new UserFriendlyException("导入失败！", "导入的数据中存在拼音码为空的行！");
            } 
            #endregion
            int sucCount=0;
            foreach (var row in rows)
            {
                //开始处理
                try
                {
                    await HandRow(row);
                    sucCount++;
                }
                catch (Exception)
                {
                     
                }
                
            }
            return $"共处理 ${rows.Count} 条数据，成功：{sucCount}条；失败：${rows.Count-sucCount}条！";
        }
            
    }


    private async Task HandRow(ReagentExcelDto row)
    {
        row.No=row.No.Trim(); 
        var entity=await this._reagentRepository.FirstOrDefaultAsync(w=>w.No==row.No);
        if (entity == null)
        {
            //新增
            entity = new Reagent();
            entity.No=row.No;
            PrePareEntity(entity,row);
            await this._reagentRepository.UpdateAsync(entity);

        }
        else
        {
            //修改
            PrePareEntity(entity,row);
            await this._reagentRepository.UpdateAsync(entity);
        }
    }
        
    public static List<EnumberEntityDto> StorageAttrList { get; set; } = CommonAppService.GetStaticStorageAttrList();

    private  void PrePareEntity(Reagent entity, ReagentExcelDto row)
    {
 
        #region 状态
        if (row.ReagentStatus.Contains("固")){
            entity.ReagentStatus = ReagentStatus.固体;
        }
        else if (row.ReagentStatus.Contains("液"))
        {
            entity.ReagentStatus = ReagentStatus.液体;
        }
        else
        {
            entity.ReagentStatus = ReagentStatus.气体;
        }
        #endregion

        #region 类型
        if (row.ReagentCatalog.Contains("专")){
            entity.ReagentCatalog = ReagentCatalog.专管试剂;
        }
        else if (row.ReagentCatalog.Contains("标"))
        {
            entity.ReagentCatalog = ReagentCatalog.标品试剂;
        }
        else
        {
            entity.ReagentCatalog = ReagentCatalog.常规试剂;
        } 
        #endregion
        entity.CasNo = row.CasNo;
        entity.CnName = row.CnName;
        entity.PinYinCode = row.PinYinCode;
        entity.CnAliasName = row.CnAliasName;
        entity.EnName = row.EnName;


        #region 安全属性
        if (!row.SafeAttribute.IsNullOrEmpty()) {
            if (row.SafeAttribute.Contains("剧毒")){
                entity.SafeAttribute = SafeAttributes.剧毒品;
            }
            else if (row.SafeAttribute.Contains("毒"))
            {
                entity.SafeAttribute = SafeAttributes.易制毒;
            }
            else if (row.SafeAttribute.Contains("爆"))
            {
                entity.SafeAttribute = SafeAttributes.易制爆;
            }
            else
            {
                entity.SafeAttribute = SafeAttributes.其它;
            }
        }
       
        #endregion


      
        entity.Price = row.Price;
        #region 存储属性
        if (!row.StorageAttr.IsNullOrEmpty())
        {
            var first = StorageAttrList.FirstOrDefault(w => w.EnumName.Contains(row.StorageAttr));
            if (first != null)
            {
                entity.StorageAttr = (StorageAttrEnum)first.EnumValue;
            }
        }  
        #endregion

        entity.Purity = row.Purity;
        entity.Capacity = row.Capacity;
        entity.CapacityUnit = row.CapacityUnit;
        entity.StorageCondition = row.StorageCondition;
        entity.InventoryWarning = row.InventoryWarning;

        if (!row.SupplierCompanyName.IsNullOrEmpty())
        {
            entity.SupplierCompanyName = row.SupplierCompanyName;
            entity.SupplierCompanyId = _companyDomainService.GetAllCompany(true)
                .FirstOrDefault(w => w.Name == row.SupplierCompanyName)?.Id;
        }
        if (!row.ProductionCompanyName.IsNullOrEmpty())
        {
            entity.ProductionCompanyName = row.SupplierCompanyName;
            entity.ProductionCompanyId = _companyDomainService.GetAllCompany(true)
                .FirstOrDefault(w => w.Name == row.ProductionCompanyName)?.Id;
        }
    }
}