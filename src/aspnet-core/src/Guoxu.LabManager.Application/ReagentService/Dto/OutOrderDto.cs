using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.ReagentService.Dto
{
    public class OutOrderDtoInputDto: PagedResultRequestFilterDto
    {
        public OutOrderStatusEnum? OutOrderStatus { get; set; }

        /// <summary>
        /// 所属仓库
        /// </summary>
        public int? WarehouseId { get; set; } 

        public OutOrderTypeEnum? OutOrderType { get; set; }
            
        public string ApplyUserName { get; set; }

        /// <summary>
        /// 是否包含订单项
        /// </summary>
        public bool InCludeItems { get; set; }
    }
     
    [AutoMap(typeof(OutOrder))]
    public class OutOrderDto : CreationAuditedEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsReleased { get; set; }
        public OutOrderStatusEnum OutOrderStatus { get; set; }

        /// <summary>
        /// 所属仓库
        /// </summary>
        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public OutOrderTypeEnum OutOrderType { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public long? ApplyUserId { get; set; }
        public string ApplyUserName { get; set; }
        public string ApplyUserUserName { get; set; }


        /// <summary>
        /// 订单项
        /// </summary>
        public  List<OutOrderMasterItemDto> OutOrderMasterItems { get; set; }
    }

    /// <summary>   
    /// 专管出库单详情
    /// </summary>
    [AutoMapFrom(typeof(OutOrderMasterItem))]
    public class OutOrderMasterItemDto : EntityDto
    {

        public decimal Price { get; set; }
        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime? StockOutTime { get; set; }
        public int OutOrderId { get; set; } 

        /// <summary>
        /// 客户端入库确认
        /// </summary>
        public bool ClientConfirm { get; set; }
        /// <summary>
        /// 双人双锁
        /// </summary>
        public bool DoubleConfirm { get; set; }


        /// <summary>
        /// 客户端入库确认 审核是否通过
        /// </summary>
        public OutOrderMasterItemStatues ClientConfirmed { get; set; }
        /// <summary>
        /// 双人双锁   审核是否通过
        /// </summary>
        public OutOrderMasterItemStatues DoubleConfirmed { get; set; }

        /// <summary>
        /// 针对那个库存
        /// </summary>
        public int? ReagentStockId { get; set; }
        //public virtual ReagentStock ReagentStock { get; set; }
        /// <summary>
        /// 二维码或条形码
        /// </summary> 
        public string ReagentStockBarCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary> 
        public string ReagentStockBatchNo { get; set; }

        /// <summary>
        /// 容量  规格 冗余自试剂基本信息
        /// </summary>
        public string ReagentStockCapacity { get; set; }


        /// <summary>   
        /// 容量单位   规格单位 冗余自试剂基本信息 
        public string ReagentStockCapacityUnit { get; set; }


        /// <summary>
        /// 安全属性
        /// 若该项选择，该试剂为专管试剂，也可不选，若该项不选择试剂为普通试剂
        /// </summary>
        public SafeAttributes? ReagentStockSafeAttribute { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public int StockoutAccount { get; set; }


        #region 如果时普通试剂的话  需要指定从哪个库位出什么试剂
        public int? ReagentId { get; set; }


        /// <summary>
        /// 冗余一下试剂的拼音码
        /// </summary> 
        public string ReagentPinYinCode { get; set; }

        /// <summary>
        /// cas号
        /// </summary> 
        public string ReagentCasNo { get; set; }

        /// <summary>
        /// 所属位置
        /// </summary>
        public int? LocationId { get; set; }

        /// <summary>
        /// 纯度
        /// </summary>
        public string ReagentPurity { get; set; }
        /// <summary>
        /// 位置名称
        /// </summary>
        public string LocationName { get; set; }
        #endregion

        #region 如果时普通试剂的话  需要指定从哪个库位出什么试剂
 
        /// <summary>
        /// 试剂编号
        /// </summary> 
        public string ReagentNo { get; set; }

        public string ReagentCnName { get; set; }
        public string ReagentCnAliasName { get; set; }

        public string ReagentEnName { get; set; }
        /// <summary>
        /// 试剂类型
        /// </summary>
        public ReagentCatalog ReagentReagentCatalog { get; set; }

 

 
        #endregion
    }

    [AutoMapFrom(typeof(OutOrderMasterItem))]
    public class AuditOutOrderMasterItemDto: OutOrderMasterItemDto
    {
       // public virtual OutOrder OutOrder { get; set; }
        public OutOrderStatusEnum OutOrderOutOrderStatus { get; set; }

        /// <summary>
        /// 所属仓库
        /// </summary>
        public int OutOrderWarehouseId { get; set; }

        public string OutOrderWarehouseName { get; set; }

        public OutOrderTypeEnum OutOrderOutOrderType { get; set; }

        /// <summary>   
        /// 申请人
        /// </summary>
        public long? ApplyUserId { get; set; } 
        public string OutOrderApplyUserName { get; set; }
        public string OutOrderApplyUserUserName { get; set; }
    }

    public class GetClientConfirmInputDto: PagedResultRequestFilterDto
    {
        public string UserName { get; set; }

        public int? WarehouseId { get; set; }

        public OutOrderMasterItemStatues Audited { get; set; }
    }

    /// <summary>
    /// 建出库单输入参数
    /// </summary>
    public class CreateOutOrderInputDto:ICustomValidate
    {
        private List<CreateOutOrderItem> items;

        /// <summary>
        /// 是什么出库单
        /// </summary>
        public OutOrderTypeEnum OutOrderType { get; set; }

        /// <summary>
        /// 所属仓库
        /// </summary>
        public int WarehouseId { get; set; }

        //public int? ApplyUserId { get; set; }
        public string ApplyUserName { get; set; }

        public List<CreateOutOrderItem> Items { 
            get => items??(items=new List<CreateOutOrderItem>()); 
            set => items = value; 
        }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (!items.Any())
            {
                throw new UserFriendlyException("请至少选择一个出库试剂");
            }

            switch (OutOrderType)
            {
                case OutOrderTypeEnum.专管试剂:
                    if (items.Any(w => !w.ReagentStockId.HasValue))
                    {
                        throw new UserFriendlyException("专管试剂出库单只允许出库专管试剂");
                    }
                    break;
                case OutOrderTypeEnum.普通试剂:
                    if (items.Any(w => !w.LocationId.HasValue))
                    {
                        throw new UserFriendlyException("普通试剂出库 需要指定出具体位置的试剂");
                    }
                    break;  
            }
        }
    }

    public class CreateOutOrderItem
    {
        /// <summary>
        /// 针对那个库存  专管
        /// </summary>
        public int? ReagentStockId { get; set; } 

        /// <summary>
        /// 出库数量
        /// </summary>
        [Range(1,int.MaxValue,ErrorMessage ="最少出库数量为1")]
        public int StockoutAccount { get; set; }



        #region 如果时普通试剂的话  需要指定从哪个库位出什么试剂
        public int? ReagentId { get; set; }

        /// <summary>
        /// 所属位置
        /// </summary>
        public int? LocationId { get; set; } 
        #endregion

    }
}
