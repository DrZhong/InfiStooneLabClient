import { AppComponentBase } from '@shared/app-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { ChangePwdComponent } from '../change-pwd/change-pwd.component';
import { ProfileSettingComponent } from './profile-setting/profile-setting.component';
import { WarehouseType } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'header-user',
  template: `

<div nz-dropdown nzPlacement="bottomRight" [nzDropdownMenu]="usermenuTpl">
  <div class="item d-flex align-items-center px-sm" nz-dropdown>
         <nz-avatar [nzSrc]="avatarUrl" nzSize="small" class="mr-sm"></nz-avatar>
         {{displayName}}
  </div>
</div>
<nz-dropdown-menu #usermenuTpl="nzDropdownMenu">
<div nz-menu class="width-sm">
<div nz-menu-item    routerLink="/app/public/profile"><i  nz-icon nzType="user" class="mr-sm"></i>个人中心</div>
<!-- <div nz-menu-item (click)="opensetting()"><i  nz-icon nzType="setting" class=" mr-sm"></i>设置</div> -->
<div nz-menu-item  (click)="changepwd()"><i  nz-icon nzType="lock" class="mr-sm"></i>修改密码</div>
<li nz-menu-divider></li>
<div nz-menu-item (click)="logout()"><i  nz-icon nzType="setting" class="mr-sm"></i> {{l('Logout')}}</div>
</div>
</nz-dropdown-menu>
  `,
})
export class HeaderUserComponent extends AppComponentBase implements OnInit {
  constructor(injector: Injector, private _authService: AppAuthService) {
    super(injector);
  }
  ngOnInit(): void {
    this.changeUser();
    abp.event.on('changeUser', () => {
      this.changeUser();
    });
  }

  changeUser() {
    let currentSelectedWarehouseName = '.';
    let currentSelectedWarehouseType = this.appSession.user.currentSelectedWarehouseType;
    switch (currentSelectedWarehouseType) {
      case WarehouseType.试剂:
        currentSelectedWarehouseName = '试剂仓库';
        break;
      case WarehouseType.耗材:
        currentSelectedWarehouseName = '耗材仓库';
        break;
      case WarehouseType.办公:
        currentSelectedWarehouseName = '办公仓库';
        break;
      default:
        break;
    }
    this.displayName = `${currentSelectedWarehouseName}/${this.appSession.user.userName}`;
  }

  logout(): void {
    this._authService.logout();
  }
  avatarUrl: string;
  displayName: string;
  opensetting() {
    this.modalHelper.createStatic(ProfileSettingComponent, {}).subscribe(x => {
      if (x) {
        window.location.reload();
      }
    })
  }
  changepwd() {
    this.modalHelper.createStatic(ChangePwdComponent, {}).subscribe(x => {
      if (x) {

        this._authService.logout();
      }
    })
  }
}
