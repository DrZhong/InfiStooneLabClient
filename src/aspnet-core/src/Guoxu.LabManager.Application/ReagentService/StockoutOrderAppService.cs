using Abp.Runtime.Session;
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.DomainServices;
using Guoxu.LabManager.ReagentService.Dto;
using Guoxu.LabManager.Runtimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.ReagentService
{
    /// <summary>
    /// 出库单
    /// </summary>
    [AbpAuthorize(
        PermissionNames.Pages_Reagent_ChuKuDan,
        PermissionNames.Pages_Reagent_ChuKuDan_Manager,
        PermissionNames.Pages_Reagent_DoubleConfirm,
        PermissionNames.Pages_Reagent_ClientConfirm)]
    public class StockoutOrderAppService:LabManagerAppServiceBase
    {
        private readonly IRepository<OutOrder> _outOrderRepository;
        private readonly IRepository<Warehouse> _warehouseRepository;   
        private readonly IRepository<Reagent> _reagentRepository;
        private readonly IRepository<ReagentStock> _reagentStockRepository;
        private readonly IRepository<NormalReagentStock> _normalReagentStockRepository;
        private readonly IRepository<NormalReagentLockedStock> _normalReagentLockedStockRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<OutOrderMasterItem> _outOrderMasterItemRepository;
        private readonly IRepository<OutOrderMasterItemAudit> _outOrderMasterItemAuditRepository;
        private readonly OutOrderDomainService _outOrderDomainService;

        public StockoutOrderAppService(
            IRepository<OutOrder> outOrderRepository
            , IRepository<Warehouse> warehouseRepository,
            IRepository<Reagent> reagentRepository,
            IRepository<ReagentStock> reagentStockRepository,
            IRepository<NormalReagentStock> normalReagentStockRepository,
            IRepository<NormalReagentLockedStock> normalReagentLockedStockRepository,
            IRepository<Location> locationRepository,
            IRepository<OutOrderMasterItem> outOrderMasterItemRepository, 
            IRepository<OutOrderMasterItemAudit> outOrderMasterItemAuditRepository, 
            OutOrderDomainService outOrderDomainService)
        {
            _outOrderRepository = outOrderRepository;
            _warehouseRepository = warehouseRepository;
            _reagentRepository = reagentRepository;
            _reagentStockRepository = reagentStockRepository;
            _normalReagentStockRepository = normalReagentStockRepository;
            _normalReagentLockedStockRepository = normalReagentLockedStockRepository;
            _locationRepository = locationRepository;
            _outOrderMasterItemRepository = outOrderMasterItemRepository;
            _outOrderMasterItemAuditRepository = outOrderMasterItemAuditRepository;
            _outOrderDomainService = outOrderDomainService;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task CancelOrder(int orderId)
        {
            var order=await this._outOrderRepository.GetAsync(orderId);
            
            if (order.OutOrderStatus != OutOrderStatusEnum.待出库)
            {
                throw new UserFriendlyException("只有待出库的订单可以取消");
            }
            //权限
            var canManage = await this.IsGrantedAsync(PermissionNames.Pages_Reagent_ChuKuDan_Manager);
            if (!canManage)
            {
                //如果不是管理员 只能取消自己的
                if(order.CreatorUserId!= AbpSession.UserId)
                {
                    throw new UserFriendlyException($"只有管理员和此出库单的创建者 {order.ApplyUserName} 才能取消此出库单");
                }
            }


            await _outOrderDomainService.CancelOrder(order);
        }


        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Reagent_ChuKuDan_Manager)]
        public async Task<PagedResultDto<OutOrderDto>> GetAllOrder(OutOrderDtoInputDto input)
        {
            var query = _outOrderRepository.GetAll()
                .WhereIf(!input.Filter.IsNullOrEmpty(), w => EF.Functions.Like(w.Id.ToString(),$"%{input.Filter}%"))
                .WhereIf(input.OutOrderStatus.HasValue,w=>w.OutOrderStatus==input.OutOrderStatus)
                .WhereIf(input.WarehouseId.HasValue,w=>w.WarehouseId==input.WarehouseId)
                .WhereIf(input.OutOrderType.HasValue,w=>w.OutOrderType==input.OutOrderType)
                .WhereIf(!input.ApplyUserName.IsNullOrEmpty(),w=>w.ApplyUser.Name.Contains(input.ApplyUserName) ||
                w.ApplyUser.UserName.Contains(input.ApplyUserName)); 

            var count=await query.CountAsync();
            if (input.InCludeItems)
            {
                query = query
                .Include(w => w.OutOrderMasterItems)
                .ThenInclude(w => w.ReagentStock);
            }
            var list=await query
                .Include(w => w.ApplyUser) 
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();



            return new PagedResultDto<OutOrderDto>(count,
                ObjectMapper.Map<List<OutOrderDto>>(list)
                );
        }

        /// <summary>
        /// 获取订单项
        /// </summary>
        /// <param name="outOrderId"></param>
        /// <returns></returns>
        public async Task<List<OutOrderMasterItemDto>> GetOutOrderMasterItems(int outOrderId)
        {
            var query =await _outOrderMasterItemRepository.GetAll()
                .Include(w=>w.ReagentStock)
                .Where(w => w.OutOrderId == outOrderId)
                .ToListAsync();

            return ObjectMapper.Map<List<OutOrderMasterItemDto>>(query);
        }

        /// <summary>
        /// 获取需要客户端确认审核的
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Reagent_ClientConfirm)]
        public async Task<PagedResultDto<AuditOutOrderMasterItemDto>> GetClientConfirm(GetClientConfirmInputDto input)
        {
            var query =   _outOrderMasterItemRepository.GetAll()
                .Where(w=>  w.ClientConfirm 
                          && w.ClientConfirmed==input.Audited)
                 .WhereIf(input.WarehouseId.HasValue, w => w.OutOrder.WarehouseId == input.WarehouseId)
                .WhereIf(!input.Filter.IsNullOrEmpty(),w=>EF.Functions.Like(w.OutOrderId.ToString(),$"%{input.Filter}%"))
                .WhereIf(!input.UserName.IsNullOrEmpty(),w=>w.OutOrder.ApplyUser.Name.Contains(input.UserName));

            if (input.Audited == OutOrderMasterItemStatues.待审核)
            {
                //如果只过滤待审核的话
                query = query.Where(w=>w.OutOrder.OutOrderStatus != OutOrderStatusEnum.取消);
            }
            var count=await query.CountAsync();
            var list =await query
                .Include(w=>w.ReagentStock)
                .Include(w=>w.OutOrder)
                .OrderBy(w => w.OutOrderId)
                .PageBy(input)
                .ToListAsync();
            return new PagedResultDto<AuditOutOrderMasterItemDto>(count,
                ObjectMapper.Map<List<AuditOutOrderMasterItemDto>>(list));
        }
        
        
        [AbpAuthorize(PermissionNames.Pages_Reagent_ClientConfirm)]
        public async Task OrderClientConfirmEd(OrderConfirmEdInputDto input)
        {
            var entity=await _outOrderMasterItemRepository
                .GetAll()
                .Include(w=>w.OutOrder) 
                .SingleAsync(w=>w.Id== input.OrderItemId);
            if (entity.OutOrder.CreatorUserId == AbpSession.UserId)
            {
                throw new UserFriendlyException("你不能审核自己的出库单！");
            }
            if (!entity.ClientConfirm)
            {
                throw new UserFriendlyException("此出库单项不需要客户端确认");
            }
            if ( entity.OutOrder.OutOrderStatus!= OutOrderStatusEnum.待出库)
            {
                //
                throw new UserFriendlyException("只要待出库状态的出库单才能审核");
            } 
            //终端确认过了
            var user = await GetCurrentUserAsync(); 
            //判断仓库管理员
            var house = await _warehouseRepository.GetAsync(entity.OutOrder.WarehouseId);
            if (house.MasterUserId.HasValue)
            {
                if (house.MasterUserId.Value != user.Id)
                {
                    throw new UserFriendlyException(
                        0,
                        $"你不是仓库 {house.Name} 的管理员，无权审核此试剂入库！",
                        "如果你确定你是此仓库的管理员，可以联系系统管理员把你设置成此仓库的管理员后即可审核！"
                        );
                }
            }
            entity.ClientConfirmed = input.AuditResult;


            _outOrderMasterItemAuditRepository.Insert(new OutOrderMasterItemAudit()
            {
                OutOrderMasterItemId=input.OrderItemId,
                ReagentStockAuditType=ReagentStockAuditTypeEnum.出库确认,
                AuditResult=input.AuditResult,
                AuditUserName=user.Name,
                AuditUserId=user.Id,
            });
            if(entity.ClientConfirmed== OutOrderMasterItemStatues.审核不通过)
            {
                await ReleaseStock(entity.ReagentStockId.Value);
            }
        }

        /// <summary>
        /// 审核不通过就可以直接释放被锁的库存了
        /// </summary>
        /// <returns></returns>
        private async Task ReleaseStock(int reagentStockId)
        {
            var s=await _reagentStockRepository.GetAsync(reagentStockId);
            s.LockedOrderId = null;
        }


        /// <summary>
        /// 获取双锁认证的
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Reagent_DoubleConfirm)]
        public async Task<PagedResultDto<AuditOutOrderMasterItemDto>> GetDoubleConfirm(GetClientConfirmInputDto input)
        {
            var query = _outOrderMasterItemRepository.GetAll()
                .Where(w =>  w.DoubleConfirm 
                            && w.DoubleConfirmed == input.Audited)
                .WhereIf(input.WarehouseId.HasValue,w=>w.OutOrder.WarehouseId == input.WarehouseId)
                .WhereIf(!input.Filter.IsNullOrEmpty(), w => EF.Functions.Like(w.OutOrderId.ToString(), $"%{input.Filter}%"))
                .WhereIf(!input.UserName.IsNullOrEmpty(), w => w.OutOrder.ApplyUser.Name.Contains(input.UserName));
           
            if(input.Audited== OutOrderMasterItemStatues.待审核)
            {
                //如果只过滤待审核的话
                query = query.Where(w=>w.OutOrder.OutOrderStatus != OutOrderStatusEnum.取消);
            }
            
            var count = await query.CountAsync();
            var list =await query
                .Include(w => w.ReagentStock)
                .Include(w => w.OutOrder) 
                .OrderBy(w => w.OutOrderId)
                .PageBy(input)
                .ToListAsync();
            return new PagedResultDto<AuditOutOrderMasterItemDto>(count,
                ObjectMapper.Map<List<AuditOutOrderMasterItemDto>>(list));
        }

        [AbpAuthorize(PermissionNames.Pages_Reagent_DoubleConfirm)]
        public async Task DoubleConfirmEd(OrderConfirmEdInputDto input)
        {
            var entity = await _outOrderMasterItemRepository
                .GetAll().Include(w => w.OutOrder)
                .SingleAsync(w => w.Id == input.OrderItemId);
            //.GetAsync(input.OrderItemId);
            if (entity.OutOrder.CreatorUserId == AbpSession.UserId)
            {
                throw new UserFriendlyException("你不能审核自己的出库单！");
            }
            if (!entity.DoubleConfirm)
            {
                throw new UserFriendlyException("此出库单项不需要客户端确认");
            }

            if (entity.OutOrder.OutOrderStatus != OutOrderStatusEnum.待出库)
            {
                //
                throw new UserFriendlyException("只要待出库状态的出库单才能审核");
            }
            if (entity.DoubleConfirmed == OutOrderMasterItemStatues.审核不通过)
            {
                throw new UserFriendlyException("此出库单项 已经被其它管理员审核不通过，你无需重新审核");
            }
            //是否已经有别人审核过了？
            var exit=  _outOrderMasterItemAuditRepository.GetAllList(w=>w.OutOrderMasterItemId==input.OrderItemId
            && w.ReagentStockAuditType==ReagentStockAuditTypeEnum.出库双人双锁); 
            if (exit.Any())
            {
                if (exit.Count() >= 2)
                {
                    throw new UserFriendlyException("此订单已经有两个人审核了,不需要重新审核");
                    //
                }
                else {
                    //还要判断是不是重复审核
                    if (exit[0].AuditUserId == AbpSession.UserId)
                    {
                        //说明你已经审核过了
                        throw new UserFriendlyException("你已经审核过此订单了，不允许同一个人重复审核同一个订单项");
                    }

                    //说明只有一个
                    entity.DoubleConfirmed = input.AuditResult;
                }
                // entity.DoubleConfirmed = true;
            }
            else
            {
                //如果第一个人就审核不通过的话，那么直接审核不通过吧
                if(input.AuditResult== OutOrderMasterItemStatues.审核不通过)
                {
                    entity.DoubleConfirmed = OutOrderMasterItemStatues.审核不通过;
                }
            }
            //终端确认过了
            var user = await GetCurrentUserAsync();
            _outOrderMasterItemAuditRepository.Insert(new OutOrderMasterItemAudit()
            {
                OutOrderMasterItemId = input.OrderItemId,
                ReagentStockAuditType = ReagentStockAuditTypeEnum.出库双人双锁,
                AuditResult = input.AuditResult,
                AuditUserName = user.Name,
                AuditUserId = user.Id,
            });
            if (entity.DoubleConfirmed == OutOrderMasterItemStatues.审核不通过)
            {
                await ReleaseStock(entity.ReagentStockId.Value);
            }
        }


        /// <summary>
        /// 获取我的订单
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<OutOrderDto>> GetMyOrder(OutOrderDtoInputDto input)
        {
            var uid = AbpSession.GetUserId();
            var query = _outOrderRepository.GetAll()
                 .Where(w=>w.CreatorUserId==uid || w.ApplyUserId==uid)
                 .WhereIf(!input.Filter.IsNullOrEmpty(), w => EF.Functions.Like(w.Id.ToString(), $"%{input.Filter}%"))
                 .WhereIf(input.OutOrderStatus.HasValue, w => w.OutOrderStatus == input.OutOrderStatus)
                 .WhereIf(input.WarehouseId.HasValue, w => w.WarehouseId == input.WarehouseId)
                 .WhereIf(input.OutOrderType.HasValue, w => w.OutOrderType == input.OutOrderType)
                 .WhereIf(!input.ApplyUserName.IsNullOrEmpty(), w => w.ApplyUser.Name.Contains(input.ApplyUserName) ||
                  w.ApplyUser.UserName.Contains(input.ApplyUserName));

            var count = await query.CountAsync();
            if (input.InCludeItems)
            {
                query = query
                .Include(w => w.OutOrderMasterItems)
                .ThenInclude(w => w.ReagentStock);
            }
            var list = await query
                .Include(w => w.ApplyUser) 
                
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();



            return new PagedResultDto<OutOrderDto>(count,
                ObjectMapper.Map<List<OutOrderDto>>(list)
                );

        }

        /// <summary>
        /// 建出库单
        /// </summary>
        /// <returns></returns>
        public async Task CreateOutOrder(CreateOutOrderInputDto input)
        {
            //创建出库单
            switch (input.OutOrderType)
            {
                case OutOrderTypeEnum.专管试剂:
                   await MasterStockOutOrder(input);
                    break;
                case OutOrderTypeEnum.普通试剂:
                   await CommonStockOutOrder(input);
                    break; 
            }
        }

        private async Task  MasterStockOutOrder(CreateOutOrderInputDto input)
        {
            var order=new OutOrder();
            var wh=await _warehouseRepository.GetAsync( input.WarehouseId);
            order.OutOrderType =  input.OutOrderType;
            order.WarehouseId = input.WarehouseId;
            order.WarehouseName = wh.Name;
            order.OutOrderStatus = OutOrderStatusEnum.待出库;
            order.ApplyUserId = AbpSession.UserId;//input.ApplyUserId.HasValue ? input.ApplyUserId.Value : AbpSession.UserId;
            order.ApplyUserName = AbpSession.GetUserName();
            var orderId=await _outOrderRepository.InsertAndGetIdAsync(order);

            foreach (var item in input.Items)
            {
                //处理订单项
                var stock=await _reagentStockRepository.GetAsync(item.ReagentStockId.Value);
                if (stock.LockedOrderId.HasValue)
                {
                    //说明此试剂正在被锁定，无法出库哦
                    throw new UserFriendlyException($"此试剂正在被出库单 {stock.LockedOrderId.Value.ToString()} 锁定，你无法出库！");
                }
                if (stock.StockStatus != ReagentStockStatusEnum.在库)
                {
                    throw new UserFriendlyException($"试剂 {stock.BarCode} 的状态不属于 【在库】");
                }
                if (stock.WarehouseId != order.WarehouseId)
                {
                    throw new UserFriendlyException($"试剂 {stock.BarCode} 不在仓库 {wh.Name} 里");
                }
                stock.LockedOrderId = orderId; //锁库存
                var regent = await _reagentRepository.GetAsync(stock.ReagentId);
                order.OutOrderMasterItems.Add(new OutOrderMasterItem()
                {
                    Price = stock.Price,
                    ClientConfirm=stock.ClientConfirm,
                    DoubleConfirm=stock.DoubleConfirm,
                    ReagentStockId=stock.Id,
                    StockoutAccount=1,
                    LocationId=stock.LocationId,
                    LocationName=stock.LocationName,
                    ReagentId = regent.Id,
                    ReagentCasNo = regent.CasNo,
                    ReagentNo = regent.No,
                    ReagentCnAliasName = regent.CnAliasName,
                    ReagentCnName = regent.CnName,
                    ReagentEnName = regent.EnName,
                    ReagentPurity = regent.Purity
                }); 
            }

            
        }
        private async Task CommonStockOutOrder(CreateOutOrderInputDto input)
        {
            var order = new OutOrder();
            var wh = await _warehouseRepository.GetAsync(input.WarehouseId);
            order.OutOrderType = input.OutOrderType;
            order.WarehouseId = input.WarehouseId;
            order.WarehouseName = wh.Name;
            order.OutOrderStatus = OutOrderStatusEnum.待出库;
            if (!input.ApplyUserName.IsNullOrEmpty())
            {
                //order.ApplyUserId = input.ApplyUserId.Value;
                var user= await UserManager.FindByNameAsync(input.ApplyUserName);
                if (user == null)
                {
                    throw new UserFriendlyException($"未找到账号为 {input.ApplyUserName} 的用户。请检测用户账号是否输入正确!");
                }
                order.ApplyUserName = user.Name;
                order.ApplyUserId = user.Id;
            }
            else
            {
                order.ApplyUserId= AbpSession.UserId;
                order.ApplyUserName = AbpSession.GetUserName();
            }
            //order.ApplyUserId = input.ApplyUserId.HasValue ? input.ApplyUserId.Value : AbpSession.UserId;
           // order.ApplyUserId = AbpSession.UserId;//input.ApplyUserId.HasValue ? input.ApplyUserId.Value : AbpSession.UserId;
           // order.ApplyUserName = AbpSession.GetUserName();
            foreach (var item in input.Items)
            {
                //处理订单项
                var stock = await _normalReagentStockRepository
                    .GetAllListAsync(w=>w.ReagentId==item.ReagentId && w.LocationId==item.LocationId);
               
                var lockedStock=await _normalReagentLockedStockRepository
                    .FirstOrDefaultAsync(w=>w.LocationId==item.LocationId && w.ReagentId==item.ReagentId);
                int lockedStockAccount = lockedStock?.LockAccount ?? 0; 

                if ((stock.Sum(w => w.RealAmount)- lockedStockAccount) < item.StockoutAccount)
                {
                    throw new UserFriendlyException($"指定库位的 试剂数量不足 {item.StockoutAccount} ,无法出库！");

                } 
                if (stock[0].WarehouseId != order.WarehouseId)
                {
                    throw new UserFriendlyException($"试剂 {stock[0].BarCode} 不在仓库 {wh.Name} 里");
                }
                var location = await _locationRepository.GetAsync(item.LocationId.Value);
                var regent = await _reagentRepository.GetAsync(item.ReagentId.Value);
                order.OutOrderMasterItems.Add(new OutOrderMasterItem()
                { 
                    ClientConfirm = false,
                    DoubleConfirm = false, 
                    ReagentId = item.ReagentId,
                    ReagentCasNo= regent.CasNo,
                    ReagentNo= regent.No,
                    ReagentCnAliasName= regent.CnAliasName,
                    ReagentCnName= regent.CnName,
                    ReagentEnName= regent.EnName,
                    ReagentPurity=regent.Purity,
                    LocationId = item.LocationId,
                    LocationName=location.Name,
                    StockoutAccount = item.StockoutAccount
                });

                //锁库存
                if (lockedStock == null)
                {
                    lockedStock = new NormalReagentLockedStock
                    {
                        ReagentId=item.ReagentId.Value,
                        LocationId=item.LocationId.Value,
                        LockAccount=item.StockoutAccount
                    };
                    await _normalReagentLockedStockRepository.InsertAsync(lockedStock);
                }
                else
                {
                    lockedStock.LockAccount += item.StockoutAccount;
                    await _normalReagentLockedStockRepository.UpdateAsync(lockedStock);
                }
            }
            await _outOrderRepository.InsertAsync(order);
        }


        public async Task<List<OutOrderMasterItemAuditDto>> GetOutOrderMasterItemAudit(int orderItemId)
        {
            var query=await _outOrderMasterItemAuditRepository.GetAllListAsync(w=>w.OutOrderMasterItemId==orderItemId);

            return ObjectMapper.Map<List<OutOrderMasterItemAuditDto>>(query);
        }

    }
}
