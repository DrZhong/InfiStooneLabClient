using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Guoxu.LabManager.Authorization
{
    public class LabManagerAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var admin = context.CreatePermission(PermissionNames.Pages_Administrator, L("系统管理"));
            var user = admin.CreateChildPermission(PermissionNames.Pages_Administrator_Users, L("用户"));
            user.CreateChildPermission(PermissionNames.Pages_Administrator_Users_Activation, L("UsersActivation"));
            admin.CreateChildPermission(PermissionNames.Pages_Administrator_Roles, L("角色")); 
            admin.CreateChildPermission(PermissionNames.Pages_Administrator_Setting, L("设置"));
            admin.CreateChildPermission(PermissionNames.Pages_Administrator_Audit, L("审计日志"));
            admin.CreateChildPermission(PermissionNames.Pages_Administrator_OrganizationUnits, L("部门管理"));
            admin.CreateChildPermission(PermissionNames.Pages_Administrator_WareHouse, L("仓库管理"));
          
            var baseInfo= context.CreatePermission(PermissionNames.Pages_BaseInfo, L("基本信息"));

            baseInfo.CreateChildPermission(PermissionNames.Pages_BaseInfo_Dict, L("数据字典"));
            baseInfo.CreateChildPermission(PermissionNames.Pages_BaseInfo_Company, L("厂商信息"));
            baseInfo.CreateChildPermission(PermissionNames.Pages_BaseInfo_Location, L("库位管理"));
            baseInfo.CreateChildPermission(PermissionNames.Pages_BaseInfo_Reagent, L("试剂管理"));


            var reagent = context.CreatePermission(PermissionNames.Pages_Reagent, L("试剂后台管理"));
            var ruku= reagent.CreateChildPermission(PermissionNames.Pages_Reagent_Ruku, L("试剂入库"));
            //Pages_Reagent_Ruku_HuiShou
            ruku.CreateChildPermission(PermissionNames.Pages_Reagent_Ruku_HuiShou, L("试剂回收"));

            var chuku= reagent.CreateChildPermission(PermissionNames.Pages_Reagent_ChuKuDan, L("创建出库单"));
            chuku.CreateChildPermission(PermissionNames.Pages_Reagent_ChuKuDan_Manager, L("出库单管理"));

            reagent.CreateChildPermission(PermissionNames.Pages_Reagent_Tongji, L("试剂统计分析"));
            reagent.CreateChildPermission(PermissionNames.Pages_Reagent_CaozuoJilu, L("试剂操作记录"));
            reagent.CreateChildPermission(PermissionNames.Pages_Reagent_KucunChaxun, L("试剂库存查询"));
            reagent.CreateChildPermission(PermissionNames.Pages_Reagent_ShiPinChaxun, L("视频监控查询"));
            reagent.CreateChildPermission(PermissionNames.Pages_Reagent_WenDuChaxun, L("温湿度监控查询"));
            reagent.CreateChildPermission(PermissionNames.Pages_Reagent_XinxiGuanli, L("试剂信息管理"));
            reagent.CreateChildPermission(PermissionNames.Pages_Reagent_Changjia, L("厂家信息管理"));
            reagent.CreateChildPermission(PermissionNames.Pages_Reagent_Kuwei, L("库位管理"));
            reagent.CreateChildPermission(PermissionNames.Pages_Reagent_DoubleConfirm, L("专管出入库双人双锁审核"));
            reagent.CreateChildPermission(PermissionNames.Pages_Reagent_ClientConfirm, L("专管出入库终端审核"));


            var consum = context.CreatePermission(PermissionNames.Pages_Consum, L("耗材后台管理"));
            consum.CreateChildPermission(PermissionNames.Pages_Consum_Ruku, L("耗材入库"));
            consum.CreateChildPermission(PermissionNames.Pages_Consum_Tongji, L("耗材统计分析"));
            consum.CreateChildPermission(PermissionNames.Pages_Consum_CaozuoJilu, L("耗材操作记录"));
            consum.CreateChildPermission(PermissionNames.Pages_Consum_KucunChaxun, L("耗材库存查询"));
            consum.CreateChildPermission(PermissionNames.Pages_Consum_ShiPinChaxun, L("视频监控查询")); 
            consum.CreateChildPermission(PermissionNames.Pages_Consum_XinxiGuanli, L("耗材信息管理"));
            consum.CreateChildPermission(PermissionNames.Pages_Consum_Changjia, L("厂家信息管理"));
            consum.CreateChildPermission(PermissionNames.Pages_Consum_Kuwei, L("库位管理"));




            var office = context.CreatePermission(PermissionNames.Pages_Office, L("办公后台管理"));
            office.CreateChildPermission(PermissionNames.Pages_Office_Ruku, L("办公入库"));
            office.CreateChildPermission(PermissionNames.Pages_Office_Tongji, L("办公统计分析"));
            office.CreateChildPermission(PermissionNames.Pages_Office_CaozuoJilu, L("办公操作记录"));
            office.CreateChildPermission(PermissionNames.Pages_Office_KucunChaxun, L("办公库存查询"));
            office.CreateChildPermission(PermissionNames.Pages_Office_ShiPinChaxun, L("视频监控查询"));
            office.CreateChildPermission(PermissionNames.Pages_Office_XinxiGuanli, L("办公信息管理"));
            office.CreateChildPermission(PermissionNames.Pages_Office_Changjia, L("厂家信息管理"));
            office.CreateChildPermission(PermissionNames.Pages_Office_Kuwei, L("库位管理"));

            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LabManagerConsts.LocalizationSourceName);
        }
    }
}
