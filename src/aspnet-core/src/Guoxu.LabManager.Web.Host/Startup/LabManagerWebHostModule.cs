using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Guoxu.LabManager.Configuration;

namespace Guoxu.LabManager.Web.Host.Startup
{
    [DependsOn(
       typeof(LabManagerWebCoreModule))]
    public class LabManagerWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public LabManagerWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LabManagerWebHostModule).GetAssembly());
        }
    }
}
