using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using Guoxu.LabManager.Domains;

namespace Guoxu.LabManager.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";


        /// <summary>
        /// 当前用户选中的登陆仓库类型
        /// </summary> 
        public  WarehouseType? CurrentSelectedWarehouseType { get; set; }

        /// <summary>
        /// 是否是超管
        /// </summary>
        public bool IsMaster { get; set; } = false;

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
