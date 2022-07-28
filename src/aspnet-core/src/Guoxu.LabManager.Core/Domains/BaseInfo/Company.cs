using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Guoxu.LabManager.Domains;


public enum CompanyType
{
    生产商=0,
    供应商=1
}

/// <summary>
/// 厂商
/// </summary>
public class Company : CreationAuditedEntity,ISoftDelete,IHasCreateUserName,IPassivable, IMustHaveWarehouseType
{
    /// <summary>
    /// 供应商类型
    /// </summary>
    public CompanyType CompanyType { get; set; }  
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 拼音码
    /// </summary>
    public string PinYin { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    public string ContactName { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string ContactPhone { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 所属供应商类型
    /// </summary>
    public WarehouseType WarehouseType { get; set; }

    public bool IsDeleted { get; set; }
    public string CreateUserName { get; set; }
    
}