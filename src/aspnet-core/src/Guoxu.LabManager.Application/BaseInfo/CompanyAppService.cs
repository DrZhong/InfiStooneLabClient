using System.Linq;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.BaseInfo.Dto;
using Guoxu.LabManager.Caching;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;

namespace Guoxu.LabManager.BaseInfo;


[AbpAuthorize(PermissionNames.Pages_BaseInfo_Company)]
public class CompanyAppService:AsyncCrudAppService<Company,CompanyDto,int, GetCompanyDto>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param> 
    public CompanyAppService(IRepository<Company, int> repository) : base(repository)
    { 
    }

    protected override IQueryable<Company> CreateFilteredQuery(GetCompanyDto input)
    {
        return base.CreateFilteredQuery(input)
            .WhereIf(!input.Filter.IsNullOrEmpty(),w=>w.Name.Contains(input.Filter)|| w.PinYin.Contains(input.Filter)) 
            .WhereIf(!input.ContactName.IsNullOrEmpty(), w => w.ContactName.Contains(input.ContactName)||  w.ContactPhone.Contains(input.ContactName))
            .WhereIf(input.CompanyType.HasValue, w => w.CompanyType == input.CompanyType.Value)

            ;
    }
}