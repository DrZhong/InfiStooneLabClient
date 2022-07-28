using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Quartz;
using Guoxu.LabManager.Domains;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Microsoft.EntityFrameworkCore;
using Abp.Notifications;
using Abp;
using Abp.Domain.Uow;
using Abp.Dapper.Repositories;

namespace Guoxu.LabManager.Jobs
{
    public class ReagentStockNotifyJob : JobBase, ISingletonDependency
    {
        private readonly INotificationPublisher _notificationPublisher; 
        private readonly IDapperRepository<ReagentStock> _dapperRepository;
        private readonly IRepository<Reagent> _reagentRepository;

        public ReagentStockNotifyJob(
            INotificationPublisher notificationPublisher,
            IDapperRepository<ReagentStock> dapperRepository, IRepository<Reagent> reagentRepository)
        {
            _notificationPublisher = notificationPublisher;
            _dapperRepository = dapperRepository;
            _reagentRepository = reagentRepository;
        }

        [UnitOfWork]
        public override async Task Execute(IJobExecutionContext context)
        {
            var now = DateTime.Now.AddDays(-1);
            string sql = $"select * from ReagentStock where StockStatus={(int)ReagentStockStatusEnum.离库} and LatestStockOutTime<@now and IsNoticed=0";
            var query = _dapperRepository.Query(sql, new
            {
                now
            });
            foreach (var item in query)
            {
                item.IsNoticed = true;
                if (!item.LatestStockOutUserId.HasValue)
                {
                    continue;
                }
                sql = "select * from Reagent where Id=@ReagentId";
                var reagent = (await _dapperRepository.QueryAsync<Reagent>(sql, new {
                    ReagentId= item.ReagentId
                })).FirstOrDefault();
                string message = $"专管试剂:{reagent.CnName}[{item.CasNo}]_{reagent.No} 已经离库超过一天，请及时归还！";
                var user = new UserIdentifier(1, item.LatestStockOutUserId.Value);
                await _notificationPublisher.PublishAsync("App.ReagentStockNotifyJob",
                        new MessageNotificationData(message),
                        severity: NotificationSeverity.Warn,
                        userIds: new[] { user }
                );
            }
            sql = $"update ReagentStock set IsNoticed=1 where StockStatus={(int)ReagentStockStatusEnum.离库} and LatestStockOutTime<@now";
            await _dapperRepository.ExecuteAsync(sql, new
            {
                now
            });
            //using (CurrentUnitOfWork.DisableFilter(LabManagerConsts.MustHaveWarehouseTypeFilterName))
            //{
            //    //  var query =
            //    //await _reagentStockRepository
            //    //.GetAll()
            //    //.Include(w => w.Reagent)
            //    //.Where(w => w.StockStatus == ReagentStockStatusEnum.离库
            //    //        && w.LatestStockOutTime < now
            //    //        && w.IsNoticed == false)
            //    //.ToListAsync();

            //} 
        }
    }
}
