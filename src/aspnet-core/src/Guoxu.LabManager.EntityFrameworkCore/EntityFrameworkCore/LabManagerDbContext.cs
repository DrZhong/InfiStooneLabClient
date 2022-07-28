using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Guoxu.LabManager.Authorization.Roles;
using Guoxu.LabManager.Authorization.Users;
using Guoxu.LabManager.MultiTenancy;
using Guoxu.LabManager.Audit;
using System.Threading;
using System.Threading.Tasks;
using Abp.Extensions;
using Guoxu.LabManager.Caching;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Runtimes;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Guoxu.LabManager.EntityFrameworkCore
{
    public class LabManagerDbContext : AbpZeroDbContext<Tenant, Role, User, LabManagerDbContext>
    {
        /* Define a DbSet for each entity of the application */ 
        public new virtual DbSet<AuditLogs> AuditLogs { get; set; }
        public virtual DbSet<WarehousePermission> WarehousePermission { get; set; }

        public virtual DbSet<Dict> Dict { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Location> Location { get; set; }   
        public virtual DbSet<Reagent> Reagent { get; set; } 
        public virtual DbSet<Warehouse> Warehouse { get; set; }
      
        public virtual DbSet<LocationStorageAttr> LocationStorageAttr { get; set; } 

        public virtual DbSet<ReagentLocation> ReagentLocation { get; set; }


        public virtual DbSet<ReagentStock> ReagentStock { get; set; }

        public virtual DbSet<ReagentStockHistory> ReagentStockHistory { get; set; }

        public virtual DbSet<ReagentOperateRecord> ReagentOperateRecord { get; set; }

            
        public virtual DbSet<NormalReagentStock> NormalReagentStock { get; set; }
        public virtual DbSet<NormalReagentOperateRecord> NormalReagentOperateRecord { get; set; }

        public virtual DbSet<ReagentStockAudit> ReagentStockAudit { get; set; }

        //出库
        public virtual DbSet<OutOrder> OutOrder { get; set; }
        public virtual DbSet<OutOrderMasterItem> OutOrderMasterItem { get; set; }
        public virtual DbSet<OutOrderMasterItemAudit> OutOrderMasterItemAudit { get; set; }

        public virtual DbSet<NormalReagentLockedStock> NormalReagentLockedStock { get; set; }
        public virtual DbSet<UserFinger> UserFinger { get; set; }

        public UserCacheDomainService UserCacheDomainService { get; set; }

       

        protected virtual WarehouseType? CurrentWarehouseType => GetCurrentWarehouseTypeOrNull();
        public LabManagerDbContext(DbContextOptions<LabManagerDbContext> options)
            : base(options)
        {   
        }

        protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
        {
            if (typeof(IMustHaveWarehouseType).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return base.ShouldFilterEntity<TEntity>(entityType);
        }
        protected virtual bool IsWarehouseTypeFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(LabManagerConsts.MustHaveWarehouseTypeFilterName) == true;
        
        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
        {
            var expression = base.CreateFilterExpression<TEntity>();

            if (typeof(IMustHaveWarehouseType).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> mayHaveOUFilter = e => ((IMustHaveWarehouseType)e).WarehouseType == CurrentWarehouseType || (((IMustHaveWarehouseType)e).WarehouseType == CurrentWarehouseType) == IsWarehouseTypeFilterEnabled;
                expression = expression == null ? mayHaveOUFilter : CombineExpressions(expression, mayHaveOUFilter);
            }

            return expression;
        }


        protected WarehouseType? GetCurrentWarehouseTypeOrNull()
        {
            return UserCacheDomainService.GetCurrentUserCache().CurrentSelectedWarehouseType;
        }
        public override int SaveChanges()
        {
            if (this.ShouldInterceptSaveChange)
            {
                OnBeforeSaving();
            }
            return base.SaveChanges();
        }

        public bool ShouldInterceptSaveChange { get; set; } = true;

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.ShouldInterceptSaveChange)
            {
                OnBeforeSaving();
            } 
            return base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is IHasCreateUserName)
                        {
                            entry.Entity.As<IHasCreateUserName>().CreateUserName = AbpSession.GetUserName();
                        }

                        if (entry.Entity is IMustHaveWarehouseType)
                        {
                            entry.Entity.As<IMustHaveWarehouseType>().WarehouseType =
                                AbpSessionExtension.GetCurrentWarehouseType();
                        }
                        break;

                    case EntityState.Modified:
                        if (entry.Entity is IHasUpdateUserName)
                        {
                            entry.Entity.As<IHasUpdateUserName>().UpdateUserName = AbpSession.GetUserName();
                        }
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is IHasDeleteUserName)
                        {
                            entry.Entity.As<IHasDeleteUserName>().DeleteUserName = AbpSession.GetUserName();
                        }
                        break;
                }
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<LocationStorageAttr>().HasIndex(w => new { w.LocationId, w.StorageAttr })
                .IsUnique(true);

            modelBuilder.Entity<Reagent>()
                .HasIndex(w => new { w.No, w.WarehouseType }).IsUnique(true);
            //modelBuilder.Entity<Medicine>().HasIndex(w => w.Barcode);

            //modelBuilder.Entity<FriendShip>().HasIndex(w => w.SourceUserId);

            //modelBuilder.Entity<User>()
            //    .HasOne(w => w.UserDetail)
            //    .WithOne(w => w.User)
            //    .HasForeignKey<UserDetail>(o => o.Id);
        }
    }
}
