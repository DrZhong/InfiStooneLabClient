using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Guoxu.LabManager.Audit;

namespace Guoxu.LabManager.Auditing
{
    [AutoMapFrom(typeof(AuditLogs))]
    public class AuditLogListDto : EntityDto<long>
    {
        public long? UserId { get; set; }
        public string CreateUserName { get; set; }
        public string UserName => this.CreateUserName;

        public int? ImpersonatorTenantId { get; set; }

        public long? ImpersonatorUserId { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string Parameters { get; set; }

        public DateTime ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        public string ClientIpAddress { get; set; }

        public string ClientName { get; set; }

        public string BrowserInfo { get; set; }

        public string Exception { get; set; }

        public string CustomData { get; set; }
    }


    public class GetAuditLogsInput : PagedResultRequestDto, IShouldNormalize
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string UserName { get; set; }

        public long? UserId { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string BrowserInfo { get; set; }

        public bool? HasException { get; set; }

        public int? MinExecutionDuration { get; set; }

        public int? MaxExecutionDuration { get; set; }

        public string Sorting { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "id DESC";
            }

            if (!StartDate.HasValue)
            {
                StartDate = DateTime.Today;
            }
            if (!EndDate.HasValue)
            {
                EndDate = DateTime.Today.AddDays(1);
            }
        }
    }
}