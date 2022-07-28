using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using Abp.AutoMapper; 
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;

namespace Guoxu.LabManager.BaseInfo.Dto;

public class GetReagentDto : PagedResultRequestFilterDto
{
    public string No { get; set; }
    public string CasNo { get; set; } 
    public string SupplierCompanyName { get; set; }
   
    public string ProductionCompanyName { get; set; }

    /// <summary>
    /// 试剂类型
    /// </summary>
    public List<ReagentCatalog> ReagentCatalogs { get; set; }


    /// <summary>
    /// 试剂状态
    /// </summary>
    public ReagentStatus? ReagentStatus { get; set; }
}

//[AutoMap(typeof(Reagent))]
public class ReagentDto : CreationAuditedEntityDto,ICustomValidate
{
    /// <summary>
    /// 客户端入库确认 审核是否通过
    /// </summary>
    public bool ClientConfirmed { get; set; }
    /// <summary>
    /// 双人双锁   审核是否通过
    /// </summary>
    public bool DoubleConfirmed { get; set; }

    /// <summary>
    /// 客户端入库确认
    /// </summary>
    public bool ClientConfirm { get; set; }
    /// <summary>
    /// 双人双锁
    /// </summary>
    public bool DoubleConfirm { get; set; }

    private List<ReagentLocationDto> _reagentLocations;

    /// <summary>
    /// 试剂编号
    /// </summary>
    [Required]
    [MaxLength(512)]
    public string No { get; set; }

    /// <summary>
    /// 试剂类型
    /// </summary>
    public ReagentCatalog ReagentCatalog { get; set; }

    /// <summary>
    /// cas号
    /// </summary>
    [MaxLength(512)]
    public string CasNo { get; set; }

    /// <summary>
    /// 试剂状态
    /// </summary>
    public ReagentStatus ReagentStatus { get; set; }

    [Required]
    [MaxLength(512)]
    public string CnName { get; set; }
    public string CnAliasName { get; set; }

    [MaxLength(1024)]
    public string EnName { get; set; }
    /// <summary>
    /// 安全属性
    /// 若该项选择，该试剂为专管试剂，也可不选，若该项不选择试剂为普通试剂
    /// </summary>
    public SafeAttributes? SafeAttribute { get; set; }

    /// <summary>
    /// 存储属性 
    /// </summary>
    public StorageAttrEnum StorageAttr { get; set; }

    [MaxLength(512)]
    public string PinYinCode { get; set; }

    /// <summary>
    /// 纯度  字典
    /// </summary>
    public string Purity { get; set; }

    /// <summary>
    /// 容量
    /// </summary>
    public string Capacity { get; set; }
    /// <summary>
    /// 容量单位 字典
    /// </summary>
    [MaxLength(32)]
    public string CapacityUnit { get; set; }

    /// <summary>
    /// 存储条件 字典
    /// </summary>
    public string StorageCondition { get; set; }
    /// <summary>
    /// 库存预警
    /// </summary>
    [Range(0,int.MaxValue,ErrorMessage ="库存预定不得小于0")]
    public int InventoryWarning { get; set; }

    #region 供应商
    public int? SupplierCompanyId { get; set; }  
    public string SupplierCompanyName { get; set; }
    #endregion


    #region 生产商
    public int? ProductionCompanyId { get; set; } 
    public string ProductionCompanyName { get; set; }
    #endregion

    /// <summary>
    /// 存储位置reagentLocationIds
    /// </summary>
    public List<ReagentLocationDto> ReagentLocations
    {
        get => _reagentLocations??=new List<ReagentLocationDto>();
        set => _reagentLocations = value;
    }

    public int[] ReagentLocationIds { get; set; } //=> this.ReagentLocations.Select(w => w.LocationId).ToArray();

    [MaxLength(512)]
    public string CreateUserName { get; set; }

    /// <summary>
    /// 参考价格
    /// </summary>
    public decimal Price { get; set; }

    public void AddValidationErrors(CustomValidationContext context)
    {
        if (this.ReagentCatalog == ReagentCatalog.专管试剂)
        {
            if (!this.SafeAttribute.HasValue)
            {
                throw new UserFriendlyException("专管试剂类型必须选择一种安全属性");
            }
        }
    }
}   

[AutoMapFrom(typeof(ReagentLocation))]
public class ReagentLocationDto : EntityDto
{
    /// <summary>
    /// 所属试剂
    /// </summary>
    public int ReagentId { get; set; } 

    /// <summary>
    /// 所属位置
    /// </summary>
    public int LocationId { get; set; }

    /// <summary>
    /// 库位所属仓库
    /// </summary>
    public int LocationWarehouseId { get; set; }

    public string LocationName { get; set; }
}