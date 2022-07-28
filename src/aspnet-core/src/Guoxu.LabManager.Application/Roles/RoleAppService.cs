using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Guoxu.LabManager.Authorization;
using Guoxu.LabManager.Authorization.Roles;
using Guoxu.LabManager.Authorization.Users;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Guoxu.LabManager.Roles
{
    [AbpAuthorize(PermissionNames.Pages_Administrator_Roles)]
    public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>//, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IRepository<WarehousePermission> _warehousePermissionRepository;
        private readonly IRepository<Warehouse> _warehouseRepository;
        public RoleAppService(IRepository<Role> repository, RoleManager roleManager, UserManager userManager, IRepository<WarehousePermission> warehousePermissionRepository, IRepository<Warehouse> warehouseRepository)
            : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _warehousePermissionRepository = warehousePermissionRepository;
            _warehouseRepository = warehouseRepository;
        }

        public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            CheckCreatePermission();

            var role = ObjectMapper.Map<Role>(input);
            role.SetNormalizedName();

            CheckErrors(await _roleManager.CreateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }

        public async Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input)
        {
            var roles = await _roleManager
                .Roles
                .WhereIf(
                    !input.Permission.IsNullOrWhiteSpace(),
                    r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                )
                .ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        public override async Task<RoleDto> UpdateAsync(RoleDto input)
        {
            CheckUpdatePermission();

            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            ObjectMapper.Map(input, role);

            CheckErrors(await _roleManager.UpdateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var role = await _roleManager.FindByIdAsync(input.Id.ToString());
            var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();

            return Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            ));
        }

        protected override IQueryable<Role> CreateFilteredQuery(PagedRoleResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Permissions)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword)
                || x.DisplayName.Contains(input.Keyword)
                || x.Description.Contains(input.Keyword));
        }

        protected override async Task<Role> GetEntityByIdAsync(int id)
        {
            return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedRoleResultRequestDto input)
        {
            return query.OrderBy(r => r.DisplayName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = new Permission[0];
            RoleEditDto roleEditDto = new RoleEditDto(); ; 

         

            if (input.Id.HasValue) //Editing existing role?
            {   
                var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
                grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                roleEditDto = ObjectMapper.Map<RoleEditDto>(role);// role.MapTo<RoleEditDto>();

           
            }

      

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList(),
                //WarehousePermissions = warehousePermissions
                //GrantedWarehousePermissions = grantedWarehousePermissions
            };
        }

        public async Task<List<WarehousePermissionContainer>> GetWarehousePermissionForEdit(int roleId)
        {
            //已有的权限
            var grantedWarehousePermissions = await _warehousePermissionRepository
                .GetAllListAsync(w => w.RoleId == roleId && w.IsActive);
 
 
            //所有仓库
            var allWarehouse = await _warehouseRepository.GetAllListAsync(w => w.IsActive);
            var warehousePermissions = new List<WarehousePermissionContainer>();

            foreach (var warehouse in allWarehouse)
            {   
                var entity = new WarehousePermissionContainer()
                {
                    WareHouseId = warehouse.Id,
                    RoleId = roleId,
                    WareHouseName = warehouse.Name
                };
                entity.Item.Add(new WarehousePermissionContainerItem()
                {
                    Permission = WarehousePermissionEnum.库存查询,
                    IsGranted = grantedWarehousePermissions.Any(w => w.RoleId == roleId
                                                                   && w.WarehouseId == warehouse.Id &&
                                                                   w.Permission == WarehousePermissionEnum.库存查询),
                });
                entity.Item.Add(new WarehousePermissionContainerItem()
                {
                    Permission = WarehousePermissionEnum.试剂入库,
                    IsGranted = grantedWarehousePermissions.Any(w => w.RoleId == roleId
                                                                     && w.WarehouseId == warehouse.Id &&
                                                                     w.Permission == WarehousePermissionEnum.试剂入库),
                });
                entity.Item.Add(new WarehousePermissionContainerItem()
                {
                    Permission = WarehousePermissionEnum.试剂领用,
                    IsGranted = grantedWarehousePermissions.Any(w => w.RoleId == roleId
                                                                     && w.WarehouseId == warehouse.Id &&
                                                                     w.Permission == WarehousePermissionEnum.试剂领用),
                });
                entity.Item.Add(new WarehousePermissionContainerItem()
                {
                    Permission = WarehousePermissionEnum.试剂归还,
                    IsGranted = grantedWarehousePermissions.Any(w => w.RoleId == roleId
                                                                     && w.WarehouseId == warehouse.Id &&
                                                                     w.Permission == WarehousePermissionEnum.试剂归还),
                });
                entity.Item.Add(new WarehousePermissionContainerItem()
                {
                    Permission = WarehousePermissionEnum.出库单,
                    IsGranted = grantedWarehousePermissions.Any(w => w.RoleId == roleId
                                                                     && w.WarehouseId == warehouse.Id &&
                                                                     w.Permission == WarehousePermissionEnum.出库单),
                });
                warehousePermissions.Add(entity);
            }

            return warehousePermissions;
        }

        /// <summary>
        /// 更新仓库权限
        /// </summary>  
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateWarehousePermission(List<WarehousePermissionContainer> input)
        { 
            var roleId = input.FirstOrDefault()?.RoleId;
            if (roleId == null) return;
            var oldAttr = await _warehousePermissionRepository
                .GetAllListAsync(w => w.RoleId == roleId);
            foreach (var locationStorageAttr in oldAttr)
            {
                locationStorageAttr.IsActive = false;
            }
            foreach (var warehousePermissionContainer in input)
            {
                foreach (var warehousePermissionContainerItem in warehousePermissionContainer.Item.Where(w=>w.IsGranted))
                {
                    var oe = oldAttr.FirstOrDefault(w =>
                        w.WarehouseId == warehousePermissionContainer.WareHouseId &&
                        w.Permission == warehousePermissionContainerItem.Permission);
                    if (oe != null)
                    {
                        oe.IsActive = true;
                    }
                    else
                    {
                        await _warehousePermissionRepository.InsertAsync(new WarehousePermission()
                        {
                            RoleId = roleId.Value,
                            WarehouseId = warehousePermissionContainer.WareHouseId,
                            Permission = warehousePermissionContainerItem.Permission,
                            IsActive = true
                        });
                    }
                }
            }
        }
    }
}

