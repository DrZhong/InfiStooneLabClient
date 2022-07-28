using Abp.Auditing;
using Abp.AutoMapper;
using Guoxu.LabManager.Domains; 
using System.ComponentModel.DataAnnotations; 


namespace Guoxu.LabManager.Audit
{
    [AutoMapFrom(typeof(AuditLog))]
    public class AuditLogs : AuditLog, IHasCreateUserName
    {
        [MaxLength(512)]
        public string CreateUserName { get; set; }
    }
}
