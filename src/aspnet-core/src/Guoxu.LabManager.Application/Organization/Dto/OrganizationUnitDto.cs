using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;
using Guoxu.LabManager.Authorization;

namespace Guoxu.LabManager.Organization.Dto
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class OrganizationUnitDto : AuditedEntityDto<long>
    { 
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sort { get; set; }

        public long? ParentId { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public int MemberCount { get; set; }
    }
}