using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace Guoxu.LabManager.Dto
{
    /// <summary>
    /// 翻页并且排序 还加一个过滤项
    /// </summary>
    public  class PagedResultRequestFilterDto:  IPagedResultRequest,ISortedResultRequest
    {
        /// <summary>
        /// 过滤
        /// </summary>
        public string Filter { get; set; }

        public string Sorting { get; set; } = "id desc";

        [Range(0, 2147483647)]
        public int SkipCount { get; set; }

        [Range(1, 2147483647)]
        public virtual int MaxResultCount { get; set; } = 10;
    }


    public class PagedResultRequestByIdDto<T> : IPagedResultRequest, ISortedResultRequest
    {
        /// <summary>
        /// 过滤
        /// </summary>
        public T Id { get; set; }

        public string Sorting { get; set; } = "id desc";

        [Range(0, 2147483647)]  
        public int SkipCount { get; set; }

        [Range(1, 2147483647)]
        public virtual int MaxResultCount { get; set; } = 10;
    }

    /// <summary>
    /// 只有一个过滤项
    /// </summary>
    public class RequestFilterDto
    {
        public string Filter { get; set; }
    }
}


