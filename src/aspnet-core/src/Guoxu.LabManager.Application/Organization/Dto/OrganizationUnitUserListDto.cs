using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Guoxu.LabManager.Authorization.Users;

namespace Guoxu.LabManager.Organization.Dto
{
    [AutoMapFrom(typeof(User))]
    public class OrganizationUnitUserListDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public DateTime AddedTime { get; set; }

        public bool IsOnline { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public long? Avatar { get; set; }

        public string AvatarUrl { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        /// 是否是部门负责人
        /// </summary>
        public bool IsOrgMaster { get; set; }

        /// <summary>
        /// 如果是hi部门负责人的话 那么把部门的code存储一下
        /// </summary>
        public string OrgMasterCode { get; set; }
    }
}