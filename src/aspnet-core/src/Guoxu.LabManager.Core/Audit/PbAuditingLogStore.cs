using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.ObjectMapping; 
using Microsoft.AspNetCore.Http;

namespace Guoxu.LabManager.Audit
{
    public class PbAuditingLogStore : IAuditingStore, ISingletonDependency
    {
        private readonly IRepository<AuditLogs, long> _auditlogRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PbAuditingLogStore(
            IRepository<AuditLogs, long> auditlogRepository,
            IObjectMapper objectMapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _auditlogRepository = auditlogRepository;
            _objectMapper = objectMapper;

            _httpContextAccessor = httpContextAccessor;
        }

        public void Save(AuditInfo auditInfo)
        {
            string abpClearException = AuditLog.GetAbpClearException(auditInfo.Exception);
            if (abpClearException.IsNullOrEmpty())
            {
                if (_httpContextAccessor.HttpContext?.Request.Method == "GET")
                {
                    //如果是get请求则不记录
                    return;
                }
            }

            var entity = _objectMapper.Map<AuditLogs>(AuditLog.CreateFromAuditInfo(auditInfo));
            _auditlogRepository.Insert(entity);
        }

        public async Task SaveAsync(AuditInfo auditInfo)
        {
            string abpClearException = AuditLog.GetAbpClearException(auditInfo.Exception);
            if (abpClearException.IsNullOrEmpty() )
            {
                if (_httpContextAccessor.HttpContext?.Request.Method == "GET")
                {
                    //如果是get请求则不记录
                    return;
                }
            }
            var entity = _objectMapper.Map<AuditLogs>(AuditLog.CreateFromAuditInfo(auditInfo));
            await _auditlogRepository.InsertAsync(entity);
        }


    }
}
