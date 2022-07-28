import { CommonServiceProxy, HomeDto, HomeMasterDto, HomeNormalDto, ReagentOperateRecordDto, ReagentStockServiceProxy, NormalReagentStockServiceProxy } from './../../shared/service-proxies/service-proxies';
import { CommonSearchComponent } from './../reagent/search/common-search/common-search.component';

import { Component, Injector, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { OnboardingService } from '@delon/abc/onboarding';
import { Observable, of } from 'rxjs';
import { STColumn } from '@delon/abc/st';
import dayjs from 'dayjs';
import { OnReuseInit, ReuseHookOnReuseInitType } from '@delon/abc/reuse-tab';
import { WarehouseType } from '@shared/service-proxies/service-proxies';
@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less'],

  animations: [appModuleAnimation()],
})
export class HomeComponent extends AppComponentBase implements OnInit, OnReuseInit {


  canViewMaterialCode = false;
  canManageUser = false;
  constructor(
    private reagentStockServiceProxy: ReagentStockServiceProxy,
    private normalReagentStockServiceProxy: NormalReagentStockServiceProxy,
    private commonServiceProxy: CommonServiceProxy,
    injector: Injector,
  ) {
    super(injector);
  }
  /**
   * 页面重新打开的时候
   * @param type 
   */
  _onReuseInit(type?: ReuseHookOnReuseInitType): void {

  }
  loading = true;

  noticationCount: number = 0;

  /**
   * 是否是专管
   */
  isMaster = false;
  currentSelectedWarehouseType: WarehouseType;
  ngOnInit(): void {
    this.isMaster = this.appSession.isMaster;
    this.currentSelectedWarehouseType = this.appSession.user.currentSelectedWarehouseType;
    this.init();

    this.home.master = new HomeMasterDto();
    this.home.normal = new HomeNormalDto();
  }



  trainScroll = {
    x: '600px',
    y: '320px'
  }
  items = [];
  init(): void {
    this.refresh();
  }

  home: HomeDto = new HomeDto();

  refresh() {
    this.masterSearch();
    this.normalSearch();
  }

  masterRecords: ReagentOperateRecordDto[] | undefined = [];;
  masterSearch() {
    if (this.isMaster) {
      this.commonServiceProxy.homeMasterDto()
        .subscribe(res => {
          this.home = res;
        });

      this.reagentStockServiceProxy.getMasterReagentOperateRecord('', '', '', undefined, undefined, undefined, undefined, undefined, undefined,
        'id desc', 0, 6).subscribe(res => {
          this.masterRecords = res.items;
        });
    }
  }
  normalRecords: ReagentOperateRecordDto[] | undefined = [];;
  normal: HomeNormalDto = new HomeNormalDto();
  normalSearch() {
    this.commonServiceProxy.homeNormalDto()
      .subscribe(res => {
        this.normal = res;
      });
    this.normalReagentStockServiceProxy.getNormalReagentOperateRecord('', '', '', undefined, undefined, undefined, undefined, undefined, undefined,
      'id desc', 0, 6).subscribe(res => {
        this.normalRecords = res.items;
      });
  }

}
