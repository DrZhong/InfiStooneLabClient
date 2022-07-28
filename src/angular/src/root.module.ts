import { NgModule, Injector } from '@angular/core';
import { CommonModule, PlatformLocation } from '@angular/common';

import { RootComponent } from 'root.component';
import { AppSessionService } from '@shared/session/app-session.service';
import { AppPreBootstrap } from 'AppPreBootstrap';
import { AppConsts } from '@shared/AppConsts';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { HttpClientModule } from '@angular/common/http';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { APP_INITIALIZER } from '@angular/core';
import { LOCALE_ID } from '@angular/core';
import { RootRoutingModule } from 'root-routing.module';
import { SharedModule } from '@shared/shared.module';
import { AppModule } from '@app/app.module';
import { registerLocaleData } from '@angular/common';
import { zhCN as dateLang } from 'date-fns/locale';
import { default as ngLang } from '@angular/common/locales/zh';
import { NZ_DATE_LOCALE, NZ_I18N, zh_CN as zorroLang } from 'ng-zorro-antd/i18n';
import { DELON_LOCALE, zh_CN as delonLang } from '@delon/theme';


const LANG = {
  abbr: 'zh',
  ng: ngLang,
  zorro: zorroLang,
  date: dateLang,
  delon: delonLang,
};

registerLocaleData(LANG.ng, LANG.abbr);
const LANG_PROVIDES = [
  // { provide: LOCALE_ID, useValue: LANG.abbr },
  { provide: NZ_I18N, useValue: LANG.zorro },
  { provide: NZ_DATE_LOCALE, useValue: LANG.date },
  { provide: DELON_LOCALE, useValue: LANG.delon },
];



export function appInitializerFactory(injector: Injector) {
  return () => {
    return new Promise<boolean>((resolve, reject) => {
      AppPreBootstrap.run(() => {
        initializeNgZorroMessage(injector);
        const appSessionService: AppSessionService = injector.get(
          AppSessionService,
        );
        appSessionService.init().then(
          result => {
            resolve(result);
          },
          err => {
            reject(err);
          },
        );
      });
    });
  };
}

// #region JSON Schema form (using @delon/form)
import { JsonSchemaModule } from '@shared/json-schema/JsonSchemaModule';
import { GlobalConfigModule } from 'global-config.module';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NgZorroMessageService } from '@shared/ui/message.service';
import { NgZorroNotifyService } from '@shared/ui/notify.service';
const FORM_MODULES = [JsonSchemaModule];
// #endregion

export function getRemoteServiceBaseUrl(): string {
  return AppConsts.remoteServiceBaseUrl;
}


export function getCurrentLanguage(): string {
  return abp.localization.currentLanguage.name;
}
function initializeNgZorroMessage(injector: Injector) {
  const zorroMessage = injector.get(NgZorroMessageService);
  zorroMessage.init();
  const zorroNotify = injector.get(NgZorroNotifyService);
  zorroNotify.init();
}
import { DelonCacheModule } from '@delon/cache';

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    GlobalConfigModule.forRoot(),
    ServiceProxyModule,
    RootRoutingModule,
    DelonCacheModule,
    SharedModule.forRoot(),

    AppModule,
    ...FORM_MODULES,
  ],
  declarations: [RootComponent],
  providers: [
    NzModalService,
    { provide: API_BASE_URL, useFactory: getRemoteServiceBaseUrl },
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializerFactory,
      deps: [Injector],
      multi: true,
    },
    {
      provide: LOCALE_ID,
      useFactory: getCurrentLanguage,
    },
    ...LANG_PROVIDES
    // { provide: NZ_ICONS, useValue: icons }
  ],
  bootstrap: [RootComponent],
})
export class RootModule { }
