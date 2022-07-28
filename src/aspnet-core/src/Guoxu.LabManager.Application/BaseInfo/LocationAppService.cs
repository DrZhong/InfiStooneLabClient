using System; 
using Abp.Application.Services; 
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.Authorization.Users;
using Guoxu.LabManager.BaseInfo.Dto;
using Guoxu.LabManager.Domains; 

namespace Guoxu.LabManager.BaseInfo;


[AbpAuthorize(PermissionNames.Pages_BaseInfo_Location)]
public class LocationAppService:AsyncCrudAppService<Location,LocationDto,int, GetLocationDto>
{
    private readonly IRepository<ReagentStock> _reagentStockRepository; 
    private readonly IRepository<LocationStorageAttr> _locationStorageAttrRepository;
    public LocationAppService(
        IRepository<Location, int> repository, 
        IRepository<LocationStorageAttr> locationStorageAttrRepository,
        IRepository<ReagentStock> reagentStockRepository) : base(repository)
    { 
        _locationStorageAttrRepository = locationStorageAttrRepository;
        _reagentStockRepository = reagentStockRepository;
    }


    public override async Task DeleteAsync(EntityDto<int> input)
    {
        var exitStock = await _reagentStockRepository
            .GetAll()
            .AnyAsync(w => w.LocationId == input.Id && w.StockStatus == ReagentStockStatusEnum.在库);

        if (exitStock)
        {
            throw new UserFriendlyException("此位置还存在 在库 状态的试剂，无法删除此库位");
        }

        await base.DeleteAsync(input);
    }




    protected override Location MapToEntity(LocationDto createInput)
    {
        var entity= base.MapToEntity(createInput);

        entity.LocationStorageAttr = this.ObjectMapper.Map<List<LocationStorageAttr>>(createInput.LocationStorageAttr);
        foreach (var locationStorageAttr in entity.LocationStorageAttr)
        {
            locationStorageAttr.IsActive = true;
        }
        return entity;
    }

    protected override void MapToEntity(LocationDto updateInput, Location entity)
    {
        base.MapToEntity(updateInput, entity);

        var oldAttr = _locationStorageAttrRepository.GetAllList(w => w.LocationId == entity.Id); //entity.LocationStorageAttr.ToList();
        foreach (var locationStorageAttr in oldAttr)
        {
            locationStorageAttr.IsActive = false;
        }

        foreach (var locationStorageAttrDto in updateInput.LocationStorageAttr)
        {
            var oe = oldAttr.FirstOrDefault(w => w.StorageAttr == locationStorageAttrDto.StorageAttr);
            if (oe != null)
            {
                oe.IsActive = true;
            }
            else
            {
                _locationStorageAttrRepository.Insert(new LocationStorageAttr()
                {
                    LocationId = entity.Id,
                    StorageAttr = locationStorageAttrDto.StorageAttr,
                    IsActive = true
                });
            }
        }

    }


    protected override  IQueryable<Location> CreateFilteredQuery(GetLocationDto input)
    { 
        return base.CreateFilteredQuery(input)
            .Include(w=>w.LocationStorageAttr)
            .Include(w=>w.Warehouse)
            .WhereIf(input.WarehouseId.HasValue,w=>w.WarehouseId==input.WarehouseId)
            .WhereIf(input.StorageAttr.HasValue,w=>w.LocationStorageAttr.Any(q=>q.StorageAttr==input.StorageAttr))
            .WhereIf(!input.Filter.IsNullOrEmpty(),w=>w.Name.Contains(input.Filter));
    }
}