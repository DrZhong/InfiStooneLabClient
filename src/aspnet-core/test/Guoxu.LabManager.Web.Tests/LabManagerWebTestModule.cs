using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Guoxu.LabManager.EntityFrameworkCore;
using Guoxu.LabManager.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Guoxu.LabManager.Web.Tests
{
    [DependsOn(
        typeof(LabManagerWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class LabManagerWebTestModule : AbpModule
    {
        public LabManagerWebTestModule(LabManagerEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LabManagerWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(LabManagerWebMvcModule).Assembly);
        }
    }
}