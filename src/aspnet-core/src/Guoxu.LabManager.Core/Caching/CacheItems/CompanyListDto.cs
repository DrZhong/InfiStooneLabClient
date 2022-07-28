using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Guoxu.LabManager.Domains;

namespace Guoxu.LabManager.Caching.CacheItems;

[AutoMapFrom(typeof(Company))]
public class CompanyListDto : EntityDto
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
    /// 状态
    /// </summary>
    public bool IsActive { get; set; }
}