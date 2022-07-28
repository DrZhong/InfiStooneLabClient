import dayjs from 'dayjs'
//import isLeapYear from 'dayjs/plugin/isLeapYear' // 导入插件
//require('dayjs/locale/zh-cn')
import 'dayjs/locale/zh-cn' // 导入本地化语言
import relativeTime from 'dayjs/plugin/relativeTime' // 导入本地化语言 
  ; // 导入本地化语言 
dayjs.extend(relativeTime);
//import * as utc from 'dayjs/plugin/utc'
//import * as timezone from 'dayjs/plugin/timezone'
//dayjs.extend(utc)
//dayjs.extend(timezone)
//dayjs.extend(isLeapYear)
dayjs.locale('zh-cn') // use locale 
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { Type, CompilerOptions, NgModuleRef } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { environment } from '@env/environment';

//import localePt from '@angular/common/locales/zh';
export class AppPreBootstrap {
  static run(callback: () => void): void {
    AppPreBootstrap.getApplicationConfig(() => {
      AppPreBootstrap.getUserConfiguration(callback);
    });
  }

  static bootstrap<TM>(
    moduleType: Type<TM>,
    compilerOptions?: CompilerOptions | CompilerOptions[],
  ): Promise<NgModuleRef<TM>> {
    return platformBrowserDynamic().bootstrapModule(
      moduleType,
      compilerOptions,
    );
  }

  private static getApplicationConfig(callback: () => void) {
    return abp
      .ajax({
        url: 'assets/' + environment.appConfig,
        method: 'GET',
        headers: {
          'Abp.TenantId': abp.multiTenancy.getTenantIdCookie(),
        },
      })
      .done(result => {
        AppConsts.appBaseUrl = result.appBaseUrl;
        AppConsts.remoteServiceBaseUrl = result.remoteServiceBaseUrl;


        callback();
      });
  }

  private static getCurrentClockProvider(
    currentProviderName: string,
  ): abp.timing.IClockProvider {
    if (currentProviderName === 'unspecifiedClockProvider') {
      return abp.timing.unspecifiedClockProvider;
    }

    if (currentProviderName === 'utcClockProvider') {
      return abp.timing.utcClockProvider;
    }

    return abp.timing.localClockProvider;
  }

  private static getUserConfiguration(
    callback: () => void,
  ): JQueryPromise<any> {
    return abp
      .ajax({
        url: AppConsts.remoteServiceBaseUrl + '/AbpUserConfiguration/GetAll',
        method: 'GET',
        headers: {
          Authorization: 'Bearer ' + abp.auth.getToken(),
          '.AspNetCore.Culture': abp.utils.getCookieValue(
            'Abp.Localization.CultureName',
          ),
          'Abp.TenantId': abp.multiTenancy.getTenantIdCookie(),
        },
      })
      .done(result => {
        $.extend(true, abp, result);

        abp.clock.provider = this.getCurrentClockProvider(
          result.clock.provider,
        );

        dayjs.locale(abp.localization.currentLanguage.name)


        if (abp.clock.provider.supportsMultipleTimezone) {
          //dayjs.tz.setDefault(abp.timing.timeZoneInfo.iana.timeZoneId);
        }
        callback();
      });
  }
}
