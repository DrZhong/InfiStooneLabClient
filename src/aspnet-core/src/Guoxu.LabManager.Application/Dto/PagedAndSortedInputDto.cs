using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Guoxu.LabManager.Dto
{
    /// <summary>
    /// 翻页
    /// </summary>
    public class PagedInputDto : IPagedResultRequest
    {
        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        [Range(1, 2147483647)]
        public int MaxResultCount { get; set; } = 20;


      
    }

    /// <summary>
    /// 翻页并且排序
    /// </summary>
    public class PagedAndSortedInputDto : IPagedResultRequest, ISortedResultRequest
    {
        public string Sorting { get; set; } = "id desc";

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        [Range(1, 2147483647)]
        public int MaxResultCount { get; set; } = 20;
    }
}

