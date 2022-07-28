using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services; 
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.Authorization.Users;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;
using Guoxu.LabManager.System.Dto; 

namespace Guoxu.LabManager.System;

[AbpAuthorize(PermissionNames.Pages_Administrator_WareHouse)] 
public class WareHouseAppService:AsyncCrudAppService<Warehouse,WareHouseDto,int,GetAll>
{
    private readonly IRepository<ReagentStock> _reagentStockRepository;
    private readonly IRepository<User, long> _userRepository;
    public WareHouseAppService(
        IRepository<Warehouse, int> repository,
        IRepository<ReagentStock> reagentStockRepository, IRepository<User, long> userRepository) : base(repository)
    {
        _reagentStockRepository = reagentStockRepository;
        _userRepository = userRepository;
    }

    protected override IQueryable<Warehouse> ApplyPaging(IQueryable<Warehouse> query, GetAll input)
    {
        return base.ApplyPaging(query, input).Include(w=>w.MasterUser);
    }


    public override async Task DeleteAsync(EntityDto<int> input)
    {
        var w=await _reagentStockRepository.GetAll()
            .AnyAsync(q=>q.WarehouseId==input.Id);
        if (w)
        {
            throw new Abp.UI.UserFriendlyException("此仓库已经包含试剂，无法执行删除仓库操作！");
        }
        await base.DeleteAsync(input);
    } 

    /// <summary>
    /// 设置仓库管理员
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task SetMaster(SetMasterDto input)
    {
        var user = await _userRepository.FirstOrDefaultAsync(w => w.UserName == input.UserPhone);
        if (user == null)
        {
            throw new UserFriendlyException($"没找到手机号为 {input.UserPhone} 的用户");
        }

        var entity = await this.Repository.GetAsync(input.WarehouseId);
        entity.MasterUserId = user.Id;
    }
}