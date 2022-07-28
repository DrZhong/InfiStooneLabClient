using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Guoxu.LabManager.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.DomainServices
{
    public class OutOrderDomainService: DomainServiceBase
    {
        private readonly IRepository<OutOrder> _outOrderRepository;
        private readonly IRepository<OutOrderMasterItem> _outOrderMasterItemRepository;
        private readonly IDapperRepository<ReagentStock> dapperRepository;
        private readonly IRepository<NormalReagentLockedStock> _normalReagentLockedStockRepository; 

        public OutOrderDomainService(IRepository<OutOrder> outOrderRepository,
            IDapperRepository<ReagentStock> dapperRepository,
            IRepository<OutOrderMasterItem> outOrderMasterItemRepository,
            IRepository<NormalReagentLockedStock> normalReagentLockedStockRepository )
        {
            _outOrderRepository = outOrderRepository;
            this.dapperRepository = dapperRepository;
            _outOrderMasterItemRepository = outOrderMasterItemRepository;
            _normalReagentLockedStockRepository = normalReagentLockedStockRepository; 
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task  CancelOrder(OutOrder item)
        {
            item.OutOrderStatus = OutOrderStatusEnum.取消;
            //其它操作
            if (item.OutOrderType == OutOrderTypeEnum.专管试剂)
            {
                await ReleaseMasterStock(item);
            }
            else
            {
                await ReleaseCommonStock(item);

            }
            await _outOrderRepository.UpdateAsync(item);
        }
        private async Task ReleaseMasterStock(OutOrder order)
        {
            string sql = @"update [ReagentStock] set [LockedOrderId]=null where [LockedOrderId]=@LockedOrderId";

            await dapperRepository.ExecuteAsync(sql, new { LockedOrderId = order.Id });
        }
        private async Task ReleaseCommonStock(OutOrder order)
        {
            var query = await _outOrderMasterItemRepository.GetAllListAsync(w => w.OutOrderId == order.Id);
            foreach (var item in query)
            {
                await ReleaseCommonStockItem(item);
            }
        }

        public async Task ReleaseCommonStockItem(OutOrderMasterItem item)
        {
            var client = await _normalReagentLockedStockRepository.FirstOrDefaultAsync(w => w.ReagentId == item.ReagentId
                    && w.LocationId == item.LocationId);

            if (client != null)
            {
                client.LockAccount -= item.StockoutAccount;
                if (client.LockAccount <= 0)
                {
                    //删除
                    await _normalReagentLockedStockRepository.DeleteAsync(client);
                }
                else
                {
                    await _normalReagentLockedStockRepository.UpdateAsync(client);
                }
            }
        }
    }
}
