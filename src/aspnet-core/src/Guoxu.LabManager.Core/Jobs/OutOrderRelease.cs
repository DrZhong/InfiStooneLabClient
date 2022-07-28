using Abp.Dapper.Repositories;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Quartz;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.DomainServices;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Jobs
{
    /// <summary>
    /// 释放出库单
    /// </summary>
    public class OutOrderRelease : JobBase, ISingletonDependency
    {

        private readonly IRepository<OutOrder> _outOrderRepository;  
        private readonly OutOrderDomainService _outOrderDomainService;
        public OutOrderRelease(IRepository<OutOrder> outOrderRepository, 
            OutOrderDomainService outOrderDomainService  )
        {
            _outOrderRepository = outOrderRepository; 
            _outOrderDomainService = outOrderDomainService;
        }

        [UnitOfWork]
        public override async Task Execute(IJobExecutionContext context)
        {
            var date = DateTime.Now.AddHours(-2);//.AddMinutes(-30);
            var query=await _outOrderRepository.
                GetAllListAsync(w=>w.OutOrderStatus== OutOrderStatusEnum.待出库  && w.CreationTime < date);
            foreach (var item in query)
            {
                await  _outOrderDomainService.CancelOrder(item); 
               
            } 
        } 
    }
}