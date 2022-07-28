using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users; 
using Abp.Organizations;  
using Abp.RealTime;
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.Authorization.Users; 
using Guoxu.LabManager.Organization.Dto; 

namespace Guoxu.LabManager.Organization
{
    [AbpAuthorize]
    public class OrganizationUnitAppService : LabManagerAppServiceBase, IOrganizationUnitAppService
    {
        private readonly IOnlineClientManager _onlineClientManager; 
        private readonly IRepository<User, long> _userRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;

        public OrganizationUnitAppService(
            OrganizationUnitManager organizationUnitManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<User, long> userRepository,  
            IOnlineClientManager onlineClientManager)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRepository = userRepository; 
            _onlineClientManager = onlineClientManager; 
        }

        public async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits()
        {

            var query =await _organizationUnitRepository.GetAll().OrderBy(w=>w.DisplayName).ToListAsync();

            var dto = ObjectMapper.Map<List<OrganizationUnitDto>>(query);//.OrderBy(w=>w.Sort).ToList();

            return new ListResultDto<OrganizationUnitDto>(dto);
        }

        public async Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input)
        {
            var allOnline = _onlineClientManager.GetAllClients().Select(w=>w.UserId).ToList();
            //var allOnlineUserIDs = allOnline.Select(w => w.UserId);
            if (input.Id.HasValue)
            {
              
                var query = from uou in _userOrganizationUnitRepository.GetAll()
                    join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                    join user in UserManager.Users.Where(w => w.IsActive)
                        .WhereIf(input.IsOnline.HasValue,w=> allOnline.Contains(w.Id))
                        .WhereIf(!input.Filter.IsNullOrEmpty(),w=> w.UserName.Contains(input.Filter) || w.Name.Contains(input.Filter)) on uou.UserId equals user.Id
                    where uou.OrganizationUnitId == input.Id
                    select new { uou, user };

                var totalCount = await query.CountAsync();
                var items = await query.OrderBy(x => x.uou.Id).PageBy(input).ToListAsync();

                return new PagedResultDto<OrganizationUnitUserListDto>(
                    totalCount,
                    items.Select(item =>
                    {
                        var dto = ObjectMapper.Map<OrganizationUnitUserListDto>(item.user);//item.user.MapTo<OrganizationUnitUserListDto>();
                        //dto.IsOrgMaster = dto.OrgMasterCode == item.uou. 
                        dto.AddedTime = item.uou.CreationTime;
                        dto.IsOnline = allOnline.Contains(item.user.Id);//.Any(q => q.UserId == item.user.Id);
                        return dto;
                    }).ToList());
            }
            else
            {
                var query =  this._userRepository.GetAll().Where(w=>w.IsActive)
                    .WhereIf(input.IsOnline.HasValue, w => allOnline.Contains(w.Id))
                    .WhereIf(!input.Filter.IsNullOrEmpty(),w=>w.UserName.Contains(input.Filter)||w.Name.Contains(input.Filter));
                var totalCount = await query.CountAsync();
                var items = await query.OrderBy(x => x.UserName).PageBy(input).ToListAsync();
                var dto = ObjectMapper.Map<List<OrganizationUnitUserListDto>>(items);
                foreach (var organizationUnitUserListDto in dto)
                {
                    organizationUnitUserListDto.IsOnline =
                        allOnline.Contains(organizationUnitUserListDto.Id);//.Any(q => q.UserId == organizationUnitUserListDto.Id);
                 
                }

                return new PagedResultDto<OrganizationUnitUserListDto>(totalCount,dto);
            }
           
        }


        [AbpAuthorize(PermissionNames.Pages_Administrator_OrganizationUnits)]
        public async Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input)
        {
            var organizationUnit =
                new OrganizationUnit(AbpSession.TenantId, input.DisplayName, input.ParentId);
            await _organizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnitDto>(organizationUnit); //organizationUnit.MapTo<OrganizationUnitDto>();
        }
 

        [AbpAuthorize(PermissionNames.Pages_Administrator_OrganizationUnits)]
        public async Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.Id);

            organizationUnit.DisplayName = input.DisplayName; ;
            await _organizationUnitManager.UpdateAsync(organizationUnit);

            return await CreateOrganizationUnitDto(organizationUnit);
        }

        [AbpAuthorize(PermissionNames.Pages_Administrator_OrganizationUnits)]
        public async Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input)
        {
            await _organizationUnitManager.MoveAsync(input.Id, input.NewParentId);

            return await CreateOrganizationUnitDto(
                await _organizationUnitRepository.GetAsync(input.Id)
                );
        }
        [AbpAuthorize(PermissionNames.Pages_Administrator_OrganizationUnits)]
        public async Task DeleteOrganizationUnit(EntityDto<long> input)
        {
            await _organizationUnitManager.DeleteAsync(input.Id);
        }
        [AbpAuthorize(PermissionNames.Pages_Administrator_OrganizationUnits)]
        public async Task AddUserToOrganizationUnit(UserToOrganizationUnitInput input)
        {
            await UserManager.AddToOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }
        [AbpAuthorize(PermissionNames.Pages_Administrator_OrganizationUnits)]
        public async Task RemoveUserFromOrganizationUnit(UserToOrganizationUnitInput input)
        {
            await UserManager.RemoveFromOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
            var user = await this._userRepository.GetAsync(input.UserId);
            
            await _userRepository.UpdateAsync(user);
        }


        public async Task<bool> IsInOrganizationUnit(UserToOrganizationUnitInput input)
        {
            return await UserManager.IsInOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }
        [AbpAuthorize(PermissionNames.Pages_Administrator_OrganizationUnits)]
        private async Task<OrganizationUnitDto> CreateOrganizationUnitDto(OrganizationUnit organizationUnit)
        {
            var dto = ObjectMapper.Map<OrganizationUnitDto>(organizationUnit); //organizationUnit.MapTo<OrganizationUnitDto>();
            dto.MemberCount = await _userOrganizationUnitRepository.CountAsync(uou => uou.OrganizationUnitId == organizationUnit.Id);
            return dto;
        }
        [AbpAuthorize(PermissionNames.Pages_Administrator_OrganizationUnits)]
        public async Task AddUsersToOrganizationUnit(UsersToOrganizationUnitInput input)
        {
            foreach (var item in input.UserIds)
            {
                await UserManager.AddToOrganizationUnitAsync(item, input.OrganizationUnitId);
            }
        }

        //AsyncUsers
    }
}