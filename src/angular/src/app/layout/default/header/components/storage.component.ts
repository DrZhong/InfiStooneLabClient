import { ConfigurationServiceProxy } from '@shared/service-proxies/service-proxies';
import { Component, HostListener, Injector } from '@angular/core';

import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'header-storage',
  template: `
  <div (click)="click()">
    <i  nz-icon nzType="tool"></i>
    {{l('ClearAllLocalStorage')}}
  </div>
  `
})
export class HeaderStorageComponent extends AppComponentBase {

  clicked: boolean = true;

  constructor(
    injector: Injector,

    private configurationServiceProxy: ConfigurationServiceProxy
  ) {
    super(injector);
  }

  click() {

  }
}
