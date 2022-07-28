import { CreateOrUpdateOrganizationComponent } from './organization/create-or-update-organization/create-or-update-organization.component';
import { OrganizationComponent } from './organization/organization.component';
import { SelectUserComponent } from './shared/select-user/select-user.component';
import { SettingComponent } from './setting/setting.component';
import { AuditDetailComponent } from './audit-log/audit-detail/audit-detail.component';
import { AuditLogComponent } from './audit-log/audit-log.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { SharedModule } from '@shared/shared.module';

import { RolesComponent } from './roles/roles.component';
import { CreatOrUpdateRoleComponent } from './roles/creat-or-update-role/creat-or-update-role.component';
import { UserComponent } from './user/user.component';
import { CreateOrUpdateUserComponent } from './user/create-or-update-user/create-or-update-user.component';
import { PermissionTreeComponent } from './shared/permission-tree.component';
import { UserBaseInfoComponent } from './user/user-base-info/user-base-info.component';
import { ResetPwdComponent } from './user/reset-pwd/reset-pwd.component';



import { SendEmailComponent } from './user/send-email/send-email.component';
import { UserDetailComponent } from './user/user-detail/user-detail.component';
import { EditUserComponent } from './user/edit-user/edit-user.component';
import { WarehouseComponent } from './warehouse/warehouse.component';
import { EditWarehouseComponent } from './warehouse/edit-warehouse/edit-warehouse.component';
import { SetAsMasterComponent } from './user/set-as-master/set-as-master.component';
import { EditClientPermissionComponent } from './roles/edit-client-permission/edit-client-permission.component';
import { SetWarehouseMasterComponent } from './warehouse/set-warehouse-master/set-warehouse-master.component';
import { UserFingerComponent } from './user-finger/user-finger.component';


const MOCKMODULE = [
  PermissionTreeComponent,
  RolesComponent,
  CreatOrUpdateRoleComponent,
  UserComponent,
  CreateOrUpdateUserComponent,
  AuditLogComponent,
  AuditDetailComponent,
  SettingComponent,
  SelectUserComponent,
  OrganizationComponent,
  CreateOrUpdateOrganizationComponent
];

@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule
  ],
  declarations: [...MOCKMODULE, UserBaseInfoComponent, ResetPwdComponent, SendEmailComponent, UserDetailComponent, EditUserComponent, WarehouseComponent, EditWarehouseComponent, SetAsMasterComponent, EditClientPermissionComponent, SetWarehouseMasterComponent, UserFingerComponent]
})
export class AdminModule { }
