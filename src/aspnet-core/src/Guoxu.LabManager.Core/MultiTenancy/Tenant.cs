using Abp.MultiTenancy;
using Guoxu.LabManager.Authorization.Users;

namespace Guoxu.LabManager.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
