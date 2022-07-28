using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.BaseInfo.Dto;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;
using Microsoft.EntityFrameworkCore;

namespace Guoxu.LabManager.BaseInfo;

/// <summary>
/// 数据字典
/// </summary>
[AbpAuthorize(PermissionNames.Pages_BaseInfo_Dict)]
public class DictAppService:AsyncCrudAppService<Dict,DictDto>
{
    
    public DictAppService(IRepository<Dict, int> repository) : base(repository)
    {
    }


    public override async Task DeleteAsync(EntityDto<int> input)
    {
        var dict =await this.GetEntityByIdAsync(input.Id);

        if (!dict.ParentId.HasValue)
        {
            throw new UserFriendlyException("父节点不允许删除");
        }

         await base.DeleteAsync(input);
    }


    


    public override async Task<PagedResultDto<DictDto>> GetAllAsync(PagedAndSortedResultRequestDto input)
    {
        var query = await base.Repository.GetAll()
            .Where(w => !w.ParentId.HasValue)
            .Include(w => w.Child)
            .ToListAsync();
        return new PagedResultDto<DictDto>(query.Count, 
            ObjectMapper.Map<List<DictDto>>(query));
    }
}