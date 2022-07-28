using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Guoxu.LabManager.Domains;

namespace Guoxu.LabManager.BaseInfo.Dto;

//[AutoMap(typeof(Dict))]
public class DictDto : EntityDto
{
    private List<DictDto> _child;

    /// <summary>
    /// 健
    /// </summary>
    [Required]
    public string Name { get; set; }
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// 顺序
    /// </summary>
    public int Sort { get; set; }

    public int? ParentId { get; set; }

    /// <summary>
    /// 子节点 
    /// </summary>
    /// <summary>
    /// 子节点 
    /// </summary>
    public List<DictDto> Child
    {
        get => _child ??= new List<DictDto>();
        set => _child = value;
    }


    public string CreateUserName { get; set; } 
}