
import { Injectable } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import {
  SessionServiceProxy,
  UserLoginInfoDto,
  TenantLoginInfoDto,
  ApplicationInfoDto,
  GetCurrentLoginInformationsOutput
} from '@shared/service-proxies/service-proxies';
import { AbpMultiTenancyService } from 'abp-ng2-module';

@Injectable()
export class AppSessionService {
  private _user: UserLoginInfoDto;
  private _tenant: TenantLoginInfoDto;
  private _application: ApplicationInfoDto;

  constructor(
    private _sessionService: SessionServiceProxy,
    private modalHelper: ModalHelper,
    private _abpMultiTenancyService: AbpMultiTenancyService,
  ) { }

  get application(): ApplicationInfoDto {
    return this._application;
  }

  get user(): UserLoginInfoDto {
    return this._user;
  }

  get userId(): number {
    return this.user ? this.user.id : null;
  }

  get tenant(): TenantLoginInfoDto {
    return this._tenant;
  }

  get tenantId(): number {
    return this.tenant ? this.tenant.id : null;
  }

  canManageReagent: boolean = false;
  canManageConsum: boolean = false;
  canManageOffice: boolean = false;
  /**
   * 是否是专管
   */
  isMaster: boolean = false;

  getShownLoginName(): string {
    const userName = this._user.userName;
    if (!this._abpMultiTenancyService.isEnabled) {
      return userName;
    }

    return (this._tenant ? this._tenant.tenancyName : '.') + '\\' + userName;
  }

  init(): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this._sessionService
        .getCurrentLoginInformations()
        .toPromise()
        .then(
          (result: GetCurrentLoginInformationsOutput) => {
            this._application = result.application;
            this._user = result.user;
            this._tenant = result.tenant;
            this.canManageConsum = result.canManageConsum;
            this.canManageOffice = result.canManageOffice;
            this.canManageReagent = result.canManageReagent;
            this.isMaster = result.user.isMaster;
            resolve(true);
          },
          err => {
            reject(err);
          },
        );
    });
  }

  changeTenantIfNeeded(tenantId?: number): boolean {
    if (this.isCurrentTenant(tenantId)) {
      return false;
    }

    abp.multiTenancy.setTenantIdCookie(tenantId);
    location.reload();
    return true;
  }

  private isCurrentTenant(tenantId?: number) {
    if (!tenantId && this.tenant) {
      return false;
    } else if (tenantId && (!this.tenant || this.tenant.id !== tenantId)) {
      return false;
    }

    return true;
  }
}
