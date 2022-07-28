using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Guoxu.LabManager.Authorization.Users;

namespace Guoxu.LabManager.Users.Dto
{
    public class AddUserToWarehouseInputDto
    {
        public long UserId { get; set; }
        public int  WareHouseId { get; set; }   
    }

    [AutoMapTo(typeof(User))]
    public class CreateUserDto : IShouldNormalize
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// ÐÕ ÔÊÐíÎª¿Õ
        /// </summary>
        //[Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }
          
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public void Normalize()
        {
            if (EmailAddress.IsNullOrEmpty())
            {
                EmailAddress = UserName + "@infistoone.com";
            }
            if (Surname.IsNullOrEmpty())
            {
                Surname = ".";
            }
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }
}
