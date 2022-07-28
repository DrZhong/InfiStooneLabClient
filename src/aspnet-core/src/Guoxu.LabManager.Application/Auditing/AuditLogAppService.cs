using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AutoMapper;
using Guoxu.LabManager.Auditing;
using Guoxu.LabManager.Audit;
using Guoxu.LabManager.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Guoxu.LabManager.Auditing
{
    [DisableAuditing]
    [AbpAuthorize(PermissionNames.Pages_Administrator_Audit)]
    public class AuditLogAppService : LabManagerAppServiceBase
    {
        private readonly IRepository<AuditLogs, long> _auditLogRepository; 
        private readonly INamespaceStripper _namespaceStripper; 

        public AuditLogAppService(
            IRepository<AuditLogs, long> auditLogRepository, 
            INamespaceStripper namespaceStripper)
        {
            _auditLogRepository = auditLogRepository; 
            _namespaceStripper = namespaceStripper;
        }

        public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        {
            var query = _auditLogRepository.GetAll()
                .WhereIf(input.StartDate.HasValue, w => w.ExecutionTime >= input.StartDate.Value)
                .WhereIf(input.EndDate.HasValue, w => w.ExecutionTime <= input.EndDate.Value)
                .WhereIf(!input.UserName.IsNullOrEmpty(), w =>w.CreateUserName==input.UserName)  
                .WhereIf(!input.MethodName.IsNullOrEmpty(), w => w.MethodName==input.MethodName)
                .WhereIf(input.HasException.HasValue, w =>  w.Exception != null);
            var count = await query.LongCountAsync();

            var list = await query.OrderByDescending(w => w.Id)
                .PageBy(input)
                .ToListAsync();
            var dto = this.ObjectMapper.Map<List<AuditLogListDto>>(list);
            foreach (var auditLogListDto in dto)
            {
                auditLogListDto.ServiceName = _namespaceStripper.StripNameSpace(auditLogListDto.ServiceName);
            }
            return new PagedResultDto<AuditLogListDto>((int)count, dto);

        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        //public async Task<UserUserDetailDto> GetUserInfo(string userName)
        //{

        //    var entity = await this.UserManager.FindByNameAsync(userName);
        //    return this.ObjectMapper.Map<UserUserDetailDto>(entity);
        //}
    }
}