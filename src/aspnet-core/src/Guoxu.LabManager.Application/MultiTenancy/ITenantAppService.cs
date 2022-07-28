using Abp.Application.Services;
using Guoxu.LabManager.MultiTenancy.Dto;

namespace Guoxu.LabManager.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

