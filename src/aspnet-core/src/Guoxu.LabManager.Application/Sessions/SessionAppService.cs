using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Domain.Repositories;
using Abp.UI;
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Sessions.Dto;
using Guoxu.LabManager.System.Dto;
using Microsoft.EntityFrameworkCore;

namespace Guoxu.LabManager.Sessions
{
    public class SessionAppService : LabManagerAppServiceBase 
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());

                output.CanManageReagent =await this.IsGrantedAsync(PermissionNames.Pages_Reagent);
                output.CanManageConsum = await this.IsGrantedAsync(PermissionNames.Pages_Consum);
                output.CanManageOffice = await this.IsGrantedAsync(PermissionNames.Pages_Office);

                //output.Warehouses = ObjectMapper.Map<List<WarehouseUsersDto>>(list);
            }

            return output;
        }

        /// <summary>
        /// 切换当前用户登陆仓库类型
        /// </summary>
        /// <param name="warehouseType"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task SwitchCurrentUserWareHouseType(WarehouseType warehouseType)
        {
            switch (warehouseType)
            {
                case WarehouseType.试剂: 
                    if (!await this.IsGrantedAsync(PermissionNames.Pages_Reagent))
                    {
                        throw new UserFriendlyException("你没有试剂后台管理权限");
                    };
                    break; 
                case WarehouseType.耗材:
                    if (!await this.IsGrantedAsync(PermissionNames.Pages_Consum))
                    {
                        throw new UserFriendlyException("你没有耗材后台管理权限");
                    };
                    break;
                case WarehouseType.办公:
                    if (!await this.IsGrantedAsync(PermissionNames.Pages_Office))
                    {
                        throw new UserFriendlyException("你没有办公后台管理权限");
                    };
                    break;
                default:
                    throw new UserFriendlyException("未知的仓库类型");
            } 
             var user = await GetCurrentUserAsync();
             user.CurrentSelectedWarehouseType = warehouseType;
        }
    }
}
