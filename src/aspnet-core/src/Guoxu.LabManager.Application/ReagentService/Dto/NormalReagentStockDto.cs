using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.AutoMapper; 
using Guoxu.LabManager.Domains;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;

namespace Guoxu.LabManager.ReagentService.Dto
{
    [AutoMapFrom(typeof(NormalReagentStock))]
    public class NormalReagentStockListDto : CreationAuditedEntityDto
    {
        /// <summary>
        /// 试剂数量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 实际在库数量
        /// </summary>
        public int RealAmount { get; set; }

        /// <summary>
        /// 二维码或条形码
        /// </summary>
        [MaxLength(512)]
        public string BarCode { get; set; }


        /// <summary>
        /// 批次
        /// </summary>
        [MaxLength(512)]
        public string BatchNo { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 保质月数
        /// </summary>
        public int ExpirationMonth { get; set; }

        /// <summary>
        /// 保质日期
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 所属位置
        /// </summary>
        public int LocationId { get; set; }
        /// <summary>
        /// 试剂的具体存储位置    
        /// </summary>
        public string LocationName { get; set; }
        //public virtual Location Location { get; set; }

        /// <summary>
        /// 所属仓库
        /// </summary>
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        //public virtual Warehouse Warehouse { get; set; }


        #region 供应商 冗余自试剂基本信息
        /// <summary>
        /// 供应商 冗余自试剂基本信息
        /// 添加的时候默认带着试剂基本信息的供应商，但是允许用户修改
        /// </summary>
        public int? SupplierCompanyId { get; set; } 
        public string SupplierCompanyName { get; set; }
        #endregion


        #region 生产商 冗余自试剂基本信息
        /// <summary>
        /// 生产商 冗余自试剂基本信息
        /// 添加的时候默认带着试剂基本信息的生产商，但是允许用户修改
        /// </summary>
        public int? ProductionCompanyId { get; set; } 
        public string ProductionCompanyName { get; set; }
        #endregion

        /// <summary>
        /// 库存状态
        /// </summary>
        public ReagentStockStatusEnum StockStatus { get; set; }

   
         

        #region 试剂信息
        /// <summary>
        /// 针对哪个试剂
        /// </summary>
        public int ReagentId { get; set; }
    
        /// <summary>
        /// 针对哪个试剂的库存
        /// </summary> 
        public string ReagentCasNo { get; set; }
        public string ReagentNo { get; set; }
        public string ReagentCnName { get; set; }
        public string ReagentCnAliasName { get; set; }
        public string ReagentEnName { get; set; }
        public string ReagentPurity { get; set; } 
        /// <summary>
        /// 冗余一下试剂的拼音码
        /// </summary>
        [MaxLength(512)]
        public string PinYinCode { get; set; }

        /// <summary>
        /// cas号
        /// </summary>
        [MaxLength(512)]
        public string CasNo { get; set; }
        #endregion
        public string CreateUserName { get; set; }

        /// <summary>
        /// 容量
        /// </summary>
        public string Capacity { get; set; }

        #region 试剂入库相关人和时间
        /// <summary>
        /// 最早试剂入库时间
        /// 第一次入库时间
        /// </summary>
        public DateTime? StockInTime { get; set; }


        public DateTime? LatestStockInTime { get; set; }

        /// <summary>
        ///  入库人 
        /// </summary>
        [MaxLength(512)]
        public string LatestStockInUserName { get; set; }
        #endregion

        #region 试剂领用相关人
        /// <summary>
        /// 最新一次出库时间
        /// 最新一次被领用事件
        /// </summary>
        public DateTime? LatestStockOutTime { get; set; }

        /// <summary>
        /// 最新一次出库人
        /// </summary>
        [MaxLength(512)]
        public string LatestStockOutUserName { get; set; }
        #endregion

        /// <summary>   
        /// 容量单位 
        /// </summary>
        [MaxLength(512)]
        public string CapacityUnit { get; set; }
        /// <summary>
        /// 参考价格
        /// </summary>
        public decimal Price { get; set; }
    }


    public class NormalReagentStockDto: NormalReagentStockListDto
    {
         
 
         
        #region 试剂入库相关人和时间
        /// <summary>
        /// 最早试剂入库时间
        /// 第一次入库时间
        /// </summary>
        public DateTime? StockInTime { get; set; }


        /// <summary>
        ///  入库人 
        /// </summary>
        [MaxLength(512)]
        public string LatestStockInUserName { get; set; }
        #endregion

        #region 试剂领用相关人
        /// <summary>
        /// 最新一次出库时间
        /// 最新一次被领用事件
        /// </summary>
        public DateTime? LatestStockOutTime { get; set; }

        /// <summary>
        /// 最新一次出库人
        /// </summary>
        [MaxLength(512)]
        public string LatestStockOutUserName { get; set; }
        #endregion 
    }

    
    public class CreateNormalReagentStockItemDto
    {
        /// <summary>
        /// 所属位置
        /// </summary>
        public int LocationId { get; set; }
        /// <summary>
        /// 入库数量    
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
         
        public string BarCode { get; set; }
    }

    [AutoMapTo(typeof(NormalReagentStock))]
    public class CreateNormalReagentStockDto : ICustomValidate
    {
        private List<CreateNormalReagentStockItemDto> items;

        public List<CreateNormalReagentStockItemDto> Items { get => items ?? (items = new List<CreateNormalReagentStockItemDto>()); set => items = value; }

        /// <summary>
        /// 批次
        /// </summary>
        [MaxLength(512)]
        [Required]
        public string BatchNo { get; set; }

        /// <summary>
        /// 参考价格
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// 供应商 冗余自试剂基本信息
        /// 添加的时候默认带着试剂基本信息的供应商，但是允许用户修改
        /// </summary>
        public int? SupplierCompanyId { get; set; }
        /// <summary>
        /// 生产商 冗余自试剂基本信息
        /// 添加的时候默认带着试剂基本信息的生产商，但是允许用户修改
        /// </summary>
        public int? ProductionCompanyId { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 保质月数
        /// </summary>
        public int ExpirationMonth { get; set; }

        /// <summary>
        /// 保质日期
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// 针对哪个试剂
        /// </summary>
        public int ReagentId { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (!ProductionDate.HasValue && !ExpirationDate.HasValue)
            {
                throw new UserFriendlyException("生产日期和过期日期必须输入一个");
            }
            if (this.Items.IsNullOrEmpty() || !this.items.Any(w=>w.Amount>0))
            {
                throw new UserFriendlyException("请至少选择一个存储库位");
            }
        }
    }
}
