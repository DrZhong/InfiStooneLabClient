using Abp.Auditing;
using Abp.Dependency;
using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Guoxu.LabManager.Audit;
using Guoxu.LabManager.Authorization.Roles;
using Guoxu.LabManager.Authorization.Users;
using Guoxu.LabManager.Configuration;
using Guoxu.LabManager.Localization;
using Guoxu.LabManager.MultiTenancy;
using Guoxu.LabManager.Timing;
using Abp.Configuration.Startup;
using Abp.Quartz;
using Abp.Threading;
using Quartz;
using Guoxu.LabManager.Jobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Guoxu.LabManager
{
    [DependsOn(typeof(AbpZeroCoreModule), typeof(AbpQuartzModule))]
    public class LabManagerCoreModule : AbpModule
    {

        private readonly IWebHostEnvironment _hostingEnvironment;

        public LabManagerCoreModule(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = false;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            LabManagerLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = LabManagerConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));

            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = LabManagerConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = LabManagerConsts.DefaultPassPhrase;

            Configuration.ReplaceService<IAuditingStore, PbAuditingLogStore>();//, DependencyLifeStyle.Singleton);

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LabManagerCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
            if (this._hostingEnvironment.IsDevelopment())
            {
                //return;
            }
            var jobManager = IocManager.Resolve<IQuartzScheduleJobManager>();

            AsyncHelper.RunSync(async () =>
            {
                await StartOutOrderReleaseJob(jobManager);
                await StartReagentStockNotifyJob(jobManager);

            });
        }

        private async Task StartOutOrderReleaseJob(IQuartzScheduleJobManager jobManager)
        {
            await jobManager.ScheduleAsync<OutOrderRelease>(
         job =>
         {
             job.WithIdentity("OutOrderRelease", "PbUser")
                 .WithDescription("A job to simply write logs.");
         },
         trigger =>
         {
             trigger
                    .StartNow()
                  .WithSimpleSchedule(job => { job.WithIntervalInMinutes(10).RepeatForever(); });

         });
        }

        private async Task StartReagentStockNotifyJob(IQuartzScheduleJobManager jobManager)
        {
            await jobManager.ScheduleAsync<ReagentStockNotifyJob>(
         job =>
         {
             job.WithIdentity("ReagentStockNotifyJob", "ReagentStockNotifyJob")
                 .WithDescription("A job to simply write logs.");
         },
         trigger =>
         {
             trigger
                    .StartNow()
                  .WithSimpleSchedule(job => { job.WithIntervalInMinutes(10).RepeatForever(); });

         });
        }
    }
}
