import { OrganizationComponent } from './organization/organization.component';
import { CommonComponent } from './../reagent/stockin/common/common.component';
import { WarehouseComponent } from './warehouse/warehouse.component';

import { AuditLogComponent } from './audit-log/audit-log.component';

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RolesComponent } from '@app/admin/roles/roles.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { UserComponent } from './user/user.component';
import { SettingComponent } from './setting/setting.component';
import { UserFingerComponent } from './user-finger/user-finger.component';
const routes: Routes = [{
  path: '',
  children: [
    {
      path: 'roles',
      component: RolesComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'auditlog',
      component: AuditLogComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'setting',
      component: SettingComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'warehouse',
      component: WarehouseComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'organization',
      component: OrganizationComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'users',
      component: UserComponent,
      canActivate: [AppRouteGuard],
      data: {
        permission: 'Pages.Administrator.Users'
      }
    },
    {
      path: 'userfinger',
      component: UserFingerComponent,
      canActivate: [AppRouteGuard],
      data: {
        permission: 'Pages.Administrator.Users'
      }
    }
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  //import { CeshiComponent } from './ceshi/ceshi.component';
  exports: [RouterModule]
})
export class AdminRoutingModule { }
