using Guoxu.LabManager.Domains.Repository;
using Abp.Dapper.Repositories;
using Guoxu.LabManager.Domains;
using Abp.Data;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using Abp.Domain.Uow;

namespace Guoxu.LabManager.EntityFrameworkCore.Repositories
{
    public class ReagentStockDapperRepository :
        DapperEfRepositoryBase<LabManagerDbContext,ReagentStock>, IReagentStockDapperRepository
    {
        public ReagentStockDapperRepository(IActiveTransactionProvider activeTransactionProvider,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider) :
            base(activeTransactionProvider, currentUnitOfWorkProvider)
        {
        }

        public class GetHomeMasterContainer
        {
            public int Num { get; set; }
            public OperateTypeEnum OperateType { get; set; }    
        }
         
        public async Task<HomeMasterDto> GetHomeMaster()
        { 
             
            var conn =  await this.GetConnectionAsync();
            var tran = await this.GetActiveTransactionAsync();
            string sql = @$"
select  
OperateType,
count(1) Num 
from ReagentOperateRecord
where CreationTime>@now
group by OperateType;
select 
count(1)
from
(select ReagentId,count(1) as Num from [dbo].[ReagentStock] where StockStatus=${(int)ReagentStockStatusEnum.在库}
group by ReagentId
) a
inner join [dbo].[Reagent]  b
on a.ReagentId =b.Id  and b.InventoryWarning >0
and b.InventoryWarning>=a.Num;
select count(1) from [dbo].[ReagentStock] where StockStatus=${(int)ReagentStockStatusEnum.在库} and DATEDIFF(day,GETDATE(),ExpirationDate) <0
";
            var queryMuti = await conn.QueryMultipleAsync(sql,new { now=System.DateTime.Today},transaction:tran);
            var query=await queryMuti.ReadAsync<GetHomeMasterContainer>();
            var n1=await queryMuti.ReadFirstOrDefaultAsync<int>();
            var n2 =await queryMuti.ReadFirstOrDefaultAsync<int>();
            return new HomeMasterDto()
            {
                TodayStockInCount = query.FirstOrDefault(w=>w.OperateType==OperateTypeEnum.入库)?.Num??0,
                TodayStockOutCount= query.FirstOrDefault(w => w.OperateType == OperateTypeEnum.领用)?.Num??0,
                TodayStockBackCount= query.FirstOrDefault(w => w.OperateType == OperateTypeEnum.归还)?.Num??0,
                TodayStockRetrieveCount= query.FirstOrDefault(w => w.OperateType == OperateTypeEnum.回收)?.Num??0,
                RegentInventoryWarningCount=n1,
                ExpirationCount=n2
            };
        }

        public async Task<HomeNormalDto> GetHomeNormal()
        {
            var conn = await this.GetConnectionAsync();
            var tran = await this.GetActiveTransactionAsync();
            string sql = @$"
select 
OperateType,
sum(OperateAmount)  Num 
from NormalReagentOperateRecord
where CreationTime>@now
group by OperateType;
 select 
count(1)
from
(select ReagentId,sum(RealAmount) as Num from NormalReagentStock where IsDeleted=0
group by ReagentId
) a
inner join [dbo].[Reagent]  b
on a.ReagentId =b.Id and b.InventoryWarning >0
and b.InventoryWarning>=a.Num;
select SUM(RealAmount) from NormalReagentStock where DATEDIFF(day,GETDATE(),ExpirationDate) <0
";
            var queryMuti = await conn.QueryMultipleAsync(sql, new { now = System.DateTime.Today }, transaction: tran);
            var query = await queryMuti.ReadAsync<GetHomeMasterContainer>();
            var n1 = await queryMuti.ReadFirstOrDefaultAsync<int>();
            var n2 = await queryMuti.ReadFirstOrDefaultAsync<int>();
            return new HomeNormalDto()
            {
                TodayStockInCount = query.FirstOrDefault(w => w.OperateType == OperateTypeEnum.入库)?.Num ?? 0,
                TodayStockOutCount = query.FirstOrDefault(w => w.OperateType == OperateTypeEnum.领用)?.Num ?? 0,
                TodayStockBackCount = query.FirstOrDefault(w => w.OperateType == OperateTypeEnum.归还)?.Num ?? 0, 
                RegentInventoryWarningCount = n1,
                ExpirationCount = n2
            };
        }
    }
}
