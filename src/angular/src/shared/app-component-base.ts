import { Injector, ElementRef, Component } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { LocalizationService, PermissionCheckerService, FeatureCheckerService, NotifyService, SettingService, MessageService, AbpMultiTenancyService } from 'abp-ng2-module';

import { AppSessionService } from '@shared/session/app-session.service';
import { ModalHelper } from '@delon/theme';

@Component({
  template: ''
})
export abstract class AppComponentBase {
  localizationSourceName = AppConsts.localization.defaultLocalizationSourceName;

  localization: LocalizationService;
  permission: PermissionCheckerService;
  feature: FeatureCheckerService;
  notify: NotifyService;
  setting: SettingService;
  message: MessageService;
  multiTenancy: AbpMultiTenancyService;
  appSession: AppSessionService;
  elementRef: ElementRef;
  modalHelper: ModalHelper;

  constructor(injector: Injector) {
    this.localization = injector.get(LocalizationService);
    this.permission = injector.get(PermissionCheckerService);
    this.feature = injector.get(FeatureCheckerService);
    this.notify = injector.get(NotifyService);
    this.setting = injector.get(SettingService);
    this.message = injector.get(MessageService);
    this.multiTenancy = injector.get(AbpMultiTenancyService);
    this.appSession = injector.get(AppSessionService);
    //this.elementRef = injector.get(ElementRef);
    this.modalHelper = injector.get(ModalHelper);
  }

  l(key: string, ...args: any[]): string {
    let localizedText = this.localization.localize(
      key,
      this.localizationSourceName,
    );

    if (!localizedText) {
      localizedText = key;
    }

    if (!args || !args.length) {
      return localizedText;
    }

    args.unshift(localizedText);
    return abp.utils.formatString.apply(this, args);
  }

  isGranted(permissionName: string): boolean {
    let permissionNames = permissionName.split(',');
    for (const ele of permissionNames) {
      if (this.permission.isGranted(ele)) return true;
    }
    return false;
    //return this.permission.isGranted(permissionName);
  }
}
