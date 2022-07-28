using System.Linq;
using System.Security.Claims;
using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.UI;
using Guoxu.LabManager.Caching;
using Guoxu.LabManager.Domains;


namespace Guoxu.LabManager.Runtimes
{
    public static class AbpSessionExtension
    {
        /// <summary>
        /// 获取用户名称
        /// </summary>
        /// <param name="abpSession"></param>
        /// <param name="principalAccessor"></param>
        /// <returns></returns>
        public static string GetUserName(this IAbpSession abpSession, IPrincipalAccessor principalAccessor = null)
        {
            if (principalAccessor == null)
            {
                principalAccessor = IocManager.Instance.Resolve<IPrincipalAccessor>();
            }
            return principalAccessor.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)
                ?.Value;
        }


        public static WarehouseType GetCurrentWarehouseType()
        {
            var userCacheDomainService = IocManager.Instance.Resolve<UserCacheDomainService>();
            var currentSelectedWarehouseType = userCacheDomainService.GetCurrentUserCache().CurrentSelectedWarehouseType;
            if (currentSelectedWarehouseType == null)
            {
                throw new UserFriendlyException("当前用户没有选中仓库类型！");
            }
            return currentSelectedWarehouseType.Value;
        }
    }
}
