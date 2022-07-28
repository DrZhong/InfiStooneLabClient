using Abp.Application.Services.Dto;
using System;

namespace Guoxu.LabManager.Users.Dto
{
    //custom PagedResultRequestDto
    public class PagedUserResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }

        public bool? IsMaster { get; set; }

        public int? RoleId { get; set; }

 
    }
}
