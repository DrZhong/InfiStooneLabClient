using Abp.Domain.Repositories; 

namespace Guoxu.LabManager.Audit
{
    public interface IAuditLogsRepository : IRepository<AuditLogs, long>
    {

    }
}
