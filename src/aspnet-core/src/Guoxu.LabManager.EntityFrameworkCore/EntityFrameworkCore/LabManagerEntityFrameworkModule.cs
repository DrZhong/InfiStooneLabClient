using Abp.Dapper;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using Guoxu.LabManager.EntityFrameworkCore.Seed;

namespace Guoxu.LabManager.EntityFrameworkCore
{
    [DependsOn(
        typeof(LabManagerCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule),
        typeof(AbpDapperModule))]
    public class LabManagerEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<LabManagerDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        LabManagerDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        LabManagerDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }


            //register filter with default value
            Configuration.UnitOfWork.RegisterFilter(LabManagerConsts.MustHaveWarehouseTypeFilterName, true);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LabManagerEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
