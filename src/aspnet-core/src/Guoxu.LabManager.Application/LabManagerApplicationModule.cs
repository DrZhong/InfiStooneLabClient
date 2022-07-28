using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Guoxu.LabManager.Authorization;

namespace Guoxu.LabManager
{
    [DependsOn(
        typeof(LabManagerCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class LabManagerApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<LabManagerAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(LabManagerApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
