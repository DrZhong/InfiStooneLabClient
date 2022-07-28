using Abp.Application.Services.Dto;

namespace Guoxu.LabManager.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

