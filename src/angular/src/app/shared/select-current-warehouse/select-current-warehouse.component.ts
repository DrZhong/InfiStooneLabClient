import { SessionServiceProxy } from './../../../shared/service-proxies/service-proxies';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, OnInit } from '@angular/core';

@Component({
  selector: 'app-select-current-warehouse',
  templateUrl: './select-current-warehouse.component.html',
  styles: [
  ]
})
export class SelectCurrentWarehouseComponent extends ModalComponentBase implements OnInit {

  constructor(
    private sessionServiceProxy: SessionServiceProxy,
    injector: Injector) {
    super(injector);
  }
  value: number;

  canManageReagent: boolean = false;
  canManageConsum: boolean = false;
  canManageOffice: boolean = false;

  ngOnInit(): void {
    this.canManageConsum = this.appSession.canManageConsum;
    this.canManageOffice = this.appSession.canManageOffice;
    this.canManageReagent = this.appSession.canManageReagent;
  }

  save() {
    this.sessionServiceProxy.switchCurrentUserWareHouseType(this.value)
      .subscribe(() => {
        location.reload();
      });
  }

}
