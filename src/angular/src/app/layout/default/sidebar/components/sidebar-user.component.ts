import { finalize } from 'rxjs/operators';
import { SessionServiceProxy, WarehouseType } from './../../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { LoadingService, LoadingType } from '@delon/abc/loading';

@Component({
  selector: 'abp-sidebar-user',
  templateUrl: './sidebar-user.component.html',
  styles: []
})
export class SidebarUserComponent extends AppComponentBase implements OnInit {



  constructor(
    injector: Injector,
    private sessionServiceProxy: SessionServiceProxy,
    private loadingSrv: LoadingService
  ) {
    super(injector);
  }
  currentSelectedWarehouseType!: WarehouseType;

  currentWarehouseName: string = '选择仓库';
  canManageReagent: boolean = false;
  canManageConsum: boolean = false;
  canManageOffice: boolean = false;
  ngOnInit() {
    this.currentSelectedWarehouseType = this.appSession.user.currentSelectedWarehouseType;
    this.canManageConsum = this.appSession.canManageConsum;
    this.canManageOffice = this.appSession.canManageOffice;
    this.canManageReagent = this.appSession.canManageReagent;
    switch (this.currentSelectedWarehouseType) {
      case WarehouseType.试剂:
        this.currentWarehouseName = '试剂仓库';
        break;
      case WarehouseType.耗材:
        this.currentWarehouseName = '耗材仓库';
        break;
      case WarehouseType.办公:
        this.currentWarehouseName = '办公仓库';
        break;
      default:
        break;
    }
  }

  select(item: WarehouseType) {
    if (this.currentSelectedWarehouseType == item) {
      return;
    }
    this.loadingSrv.open({ text: '切换中...' });
    this.sessionServiceProxy.switchCurrentUserWareHouseType(item)
      .pipe(finalize(() => {
        this.loadingSrv.close();
      }))
      .subscribe(() => {
        this.notify.success('切换成功');
        location.reload();
      });
  }
}
