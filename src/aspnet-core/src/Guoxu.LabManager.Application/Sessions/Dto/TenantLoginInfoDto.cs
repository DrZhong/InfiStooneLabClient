using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Guoxu.LabManager.MultiTenancy;

namespace Guoxu.LabManager.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
