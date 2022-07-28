using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;

namespace Guoxu.LabManager.BaseInfo.Dto;

public class GetCompanyDto : PagedResultRequestFilterDto
{
    /// <summary>
    /// 供应商类型
    /// </summary>
    public CompanyType? CompanyType { get; set; }
 

    /// <summary>
    /// 联系人
    /// </summary>
    public string ContactName { get; set; }
}

[AutoMap(typeof(Company))]
public class CompanyDto : CreationAuditedEntityDto
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

     
    public string CreateUserName { get; set; }

}

