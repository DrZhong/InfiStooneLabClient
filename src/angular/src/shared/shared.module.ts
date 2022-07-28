import { ReagentStatusPipe } from './pipes/reagent-status.pipe';
import { SafeAttributePipe } from './pipes/safe-attribute.pipe';

import { SelectWarehouseComponent } from './components/select-warehouse/select-warehouse.component';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppSessionService } from '@shared/session/app-session.service';
import { AppUrlService } from '@shared/nav/app-url.service';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
// delon
import { AlainThemeModule } from '@delon/theme';

//import { DelonACLModule } from '@delon/acl';
import { DelonFormModule } from '@delon/form';


import { SHARED_DELON_MODULES } from './shared-delon.module';
import { SHARED_ZORRO_MODULES } from './shared-zorro.module';
import { SortPipe } from '@app/sort.pipe';;
import { NgZorroMessageService } from './ui/message.service';
import { NgZorroNotifyService } from './ui/notify.service';


import { FileDownloadService } from './utils/file-download.service';
import { LocalizePipe } from './pipes/localize.pipe';
import { TimeLocalizePipe } from './pipes/time-localize.pipe';
import { SelectProductionCompanyComponent } from './components/select-production-company/select-production-company.component';
import { SelectSupplierCompanyComponent } from './components/select-supplier-company/select-supplier-company.component';
import { ReagentCatalogPipe } from './pipes/reagent-catalog.pipe';
import { StorageAttrPipe } from './pipes/storage-attr.pipe';
import { SelectStorageConditionComponent } from './components/select-storage-condition/select-storage-condition.component';
import { SelectPurityComponent } from './components/select-purity/select-purity.component';
import { OperateTypePipe } from './pipes/operate-type.pipe';
// import dayGridPlugin from '@fullcalendar/daygrid'; // a plugin
// import interactionPlugin from '@fullcalendar/interaction'; // a plugin
// import timegridPlugin from '@fullcalendar/timegrid';
// FullCalendarModule.registerPlugins([ // register FullCalendar plugins
//   dayGridPlugin,
//   interactionPlugin,
//   timegridPlugin
// ]);
// #region your componets & directives
//import { NgxEchartsModule } from 'ngx-echarts';
const COMPONENTS = [
  SelectWarehouseComponent,
  SelectProductionCompanyComponent,
  SelectSupplierCompanyComponent,
  SelectPurityComponent,
  SelectStorageConditionComponent
];
const DIRECTIVES = [
  SortPipe,
  LocalizePipe,
  TimeLocalizePipe,
  ReagentCatalogPipe,
  StorageAttrPipe,
  SafeAttributePipe,
  ReagentStatusPipe,
  OperateTypePipe
];
// #endregion

const THIRDMODULES = [
  //NgxEchartsModule
];
// endregion

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,

    AlainThemeModule.forChild(),
    //DelonACLModule,
    DelonFormModule,
    ...SHARED_DELON_MODULES,
    ...SHARED_ZORRO_MODULES,
    ...THIRDMODULES
    //FullCalendarModule
  ],
  declarations: [
    // your components
    ...COMPONENTS,
    ...DIRECTIVES
  ],
  exports: [
    SortPipe,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    AlainThemeModule,
    //DelonACLModule,
    DelonFormModule,
    ...SHARED_DELON_MODULES,
    ...SHARED_ZORRO_MODULES,
    // third libs
    ...THIRDMODULES,
    // your components
    ...COMPONENTS,
    ...DIRECTIVES,
    //FullCalendarModule
  ],
  providers: [
    FileDownloadService
  ]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders<SharedModule> {
    return {
      ngModule: SharedModule,
      providers: [
        AppSessionService,
        AppUrlService,
        AppAuthService,
        AppRouteGuard,
        NgZorroMessageService,
        NgZorroNotifyService
      ],
    };
  }
}
