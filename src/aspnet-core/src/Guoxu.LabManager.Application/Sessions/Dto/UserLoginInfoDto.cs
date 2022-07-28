using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Guoxu.LabManager.Authorization.Users;
using Guoxu.LabManager.Domains;

namespace Guoxu.LabManager.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }


        /// <summary>
        /// 当前用户选中的登陆仓库
        /// </summary>
        public WarehouseType? CurrentSelectedWarehouseType { get; set; }
        /// <summary>
        /// 是否是超管
        /// </summary>
        public bool IsMaster { get; set; } = false;
    }
}
